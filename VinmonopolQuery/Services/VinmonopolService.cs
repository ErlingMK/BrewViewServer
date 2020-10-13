using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BrewView.DatabaseModels;
using BrewView.DatabaseModels.Models;
using BrewView.DatabaseModels.Vinmonopol;
using Newtonsoft.Json;
using VinmonopolQuery.Util;

namespace VinmonopolQuery.Services
{
    public class VinmonopolService : IVinmonopolService
    {
        private readonly HttpClient m_client;
        private readonly BrewContext m_db;

        public VinmonopolService(BrewContext db, HttpClient client)
        {
            m_db = db;
            m_client = client;
            m_client.Timeout = TimeSpan.FromSeconds(240);
        }

        public async Task<int> GetProducts(bool getAll = false, string dateTime = "")
        {
            int count;
            try
            {
                Logger.Log("Fetching from vinmonopolet...");

                var httpRequestBuilder = new HttpRequestBuilder().WithMethod(HttpMethod.Get)
                    .WithRequestUri($"{Program.AppSettings.ApiUrl}{Program.AppSettings.ProductDetailsEndPoint}")
                    .AddHeader(Program.AppSettings.ApiKeyName, Program.AppSettings.ApiKey)
                    .AddQueryParameter("maxResults", 30000);

                if (!getAll) httpRequestBuilder.AddQueryParameter("changedSince", dateTime);

                var response = await m_client.SendAsync(httpRequestBuilder.Build());

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var alcoholicEntities = JsonConvert.DeserializeObject<List<AlcoholicEntity>>(content);

                count = alcoholicEntities.Count;

                Logger.Log($"Retrieved count: {count}");

                if (!getAll) await UpdateProducts(alcoholicEntities);
                else await AddAllToDb(alcoholicEntities);
            }
            catch (Exception e)
            {
                Logger.Log($"{e.Message}\n{e.StackTrace}");
                return -1;
            }

            return 0;
        }

        private async Task UpdateProducts(List<AlcoholicEntity> alcoholicEntities)
        {
            var newBrews = new List<AlcoholicEntity>();
            foreach (var alcoholicEntity in alcoholicEntities)
            {
                var find = await m_db.AlcoholicEntities.FindAsync(alcoholicEntity.Basic.ProductId);
                var brew = await m_db.Brews.FindAsync(alcoholicEntity.Basic.ProductId);

                if (find == null)
                {
                    newBrews.Add(alcoholicEntity);
                }
                else
                {
                    alcoholicEntity.ProductId = alcoholicEntity.Basic.ProductId;
                    find = alcoholicEntity;
                    brew = CreateBrew(alcoholicEntity);
                }
            }

            await m_db.SaveChangesAsync();

            await AddAllToDb(newBrews);
        }

        private Brew CreateBrew(AlcoholicEntity alcoholicEntity)
        {
            var first = alcoholicEntity.Logistics.Barcodes.FirstOrDefault(barcode => barcode.IsMainGtin);
            return new Brew
            {
                Gtin = first?.Gtin ?? "",
                ProductId = alcoholicEntity.Basic.ProductId
            };
        }

        private async Task AddAllToDb(List<AlcoholicEntity> entity)
        {
            foreach (var alcoholicEntity in entity)
            {
                alcoholicEntity.ProductId = alcoholicEntity.Basic.ProductId;
                await CreateFood(alcoholicEntity);
                await CreateGrape(alcoholicEntity);
            }

            var brews = entity.Select(CreateBrew).ToList();

            await CreateFoodBrews(entity);
            await CreateGrapeBrews(entity);
            m_db.Brews.AddRange(brews);
            m_db.AlcoholicEntities.AddRange(entity);

            await m_db.SaveChangesAsync();

            Logger.Log($"Added brews: {brews.Count}");
            Logger.Log($"Added products: {entity.Count}");
        }

        private async Task CreateGrapeBrews(List<AlcoholicEntity> entity)
        {
            var grapes = new List<GrapeAlcoholicEntity>();
            foreach (var alcoholicEntity in entity)
                grapes.AddRange(alcoholicEntity.Ingredients.Grapes.Select(grape => new GrapeAlcoholicEntity
                    {GrapeId = grape.GrapeId, ProductId = alcoholicEntity.ProductId, GrapePercent = grape.GrapePct}));

            m_db.GrapeBrews.AddRange(grapes);
            await m_db.SaveChangesAsync();
            Logger.Log($"Added grapebrews: {grapes.Count}");
        }

        private async Task CreateFoodBrews(IList<AlcoholicEntity> alcoholicEntities)
        {
            var foods = new List<FoodAlcoholicEntity>();
            foreach (var alcoholicEntity in alcoholicEntities)
                foods.AddRange(alcoholicEntity.Description.RecommendedFood.Select(food => new FoodAlcoholicEntity
                    {FoodId = food.FoodId, ProductId = alcoholicEntity.ProductId}));

            m_db.FoodBrews.AddRange(foods);
            await m_db.SaveChangesAsync();
            Logger.Log($"Added foodbrews: {foods.Count}");
        }

        private async Task CreateGrape(AlcoholicEntity alcoholicEntity)
        {
            foreach (var grape in alcoholicEntity.Ingredients.Grapes)
            {
                var find = await m_db.Grapes.FindAsync(grape.GrapeId);
                if (find == null)
                {
                    m_db.Grapes.Add(grape);
                    await m_db.SaveChangesAsync();
                    Logger.Log($"Grape created: {grape.GrapeDesc}");
                }
            }
        }

        private async Task CreateFood(AlcoholicEntity entity)
        {
            foreach (var food in entity.Description.RecommendedFood)
            {
                var find = await m_db.Foods.FindAsync(food.FoodId);
                if (find == null)
                {
                    m_db.Foods.Add(food);
                    await m_db.SaveChangesAsync();
                    Logger.Log($"Food created: {food.FoodDesc}");
                }
            }
        }
    }
}