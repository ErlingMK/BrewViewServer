using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BrewViewServer.Models;
using BrewViewServer.Models.VinmonopolModels;
using BrewViewServer.Util;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BrewViewServer.Repositories
{
    public class VinmonopolRepository : IVinmonopolRepository
    {
        private readonly IConfiguration m_configuration;
        private readonly BrewContext m_db;
        private readonly HttpClient m_client;

        public VinmonopolRepository(BrewContext db, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            m_db = db;
            m_configuration = configuration;
            m_client = clientFactory.CreateClient();
        }

        public async Task<int> GetAllProducts()
        {
            int count;
            try
            {
                //TODO: Create custom client for this instead
                var response = await m_client.SendAsync(new HttpRequestBuilder().WithMethod(HttpMethod.Get)
                    .WithRequestUri($"{AppConstants.ApiUrl}{AppConstants.ProductDetailsEndPoint}")
                    .AddHeader("Ocp-Apim-Subscription-Key", $"{m_configuration["Vinmonopolet:ApiKey"]}")
                    .AddQueryParameter("maxResults", 30000)
                    .Build());

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var alcoholicEntities = JsonConvert.DeserializeObject<List<AlcoholicEntity>>(content);

                count = alcoholicEntities.Count;

                await AddAllToDb(alcoholicEntities);
            }
            catch (Exception)
            {
                //TODO: Learn how logging works
                return -1;
            }

            return count;
        }

        public async Task<int> GetProductsUpdatedSince(DateTime dateTime)
        {
            int count;
            try
            {
                //TODO: Create custom client for this instead
                var response = await m_client.SendAsync(new HttpRequestBuilder().WithMethod(HttpMethod.Get)
                    .WithRequestUri($"{AppConstants.ApiUrl}{AppConstants.ProductDetailsEndPoint}")
                    .AddHeader("Ocp-Apim-Subscription-Key", $"{m_configuration["Vinmonopolet:ApiKey"]}")
                    .AddQueryParameter("maxResults", 30000)
                    .AddQueryParameter("changedSince", dateTime.ToString("yyyy-MM-dd"))
                    .Build());

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var alcoholicEntities = JsonConvert.DeserializeObject<List<AlcoholicEntity>>(content);

                count = alcoholicEntities.Count;

                await UpdatedProducts(alcoholicEntities);
            }
            catch (Exception)
            {
                //TODO: Learn how logging works
                return -1;
            }

            return count;
        }

        private async Task UpdatedProducts(List<AlcoholicEntity> alcoholicEntities)
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
            entity.ForEach(async alcoholicEntity =>
            {
                alcoholicEntity.ProductId = alcoholicEntity.Basic.ProductId;
                await CreateFood(alcoholicEntity);
                await CreateGrape(alcoholicEntity);
            });

            var brews = entity.Select(CreateBrew).ToList();

            await CreateFoodBrews(entity);
            await CreateGrapeBrews(entity);
            m_db.Brews.AddRange(brews);
            m_db.AlcoholicEntities.AddRange(entity);

            await m_db.SaveChangesAsync();
        }

        private async Task CreateGrapeBrews(List<AlcoholicEntity> entity)
        {
            var grapes = new List<GrapeAlcoholicEntity>();
            foreach (var alcoholicEntity in entity)
                grapes.AddRange(alcoholicEntity.Ingredients.Grapes.Select(grape => new GrapeAlcoholicEntity
                    {GrapeId = grape.GrapeId, ProductId = alcoholicEntity.ProductId, GrapePercent = grape.GrapePct}));

            m_db.GrapeBrews.AddRange(grapes);
            await m_db.SaveChangesAsync();
        }

        private async Task CreateFoodBrews(List<AlcoholicEntity> arg)
        {
            var foods = new List<FoodAlcoholicEntity>();
            foreach (var alcoholicEntity in arg)
                foods.AddRange(alcoholicEntity.Description.RecommendedFood.Select(food => new FoodAlcoholicEntity
                    {FoodId = food.FoodId, ProductId = alcoholicEntity.ProductId}));

            m_db.FoodBrews.AddRange(foods);
            await m_db.SaveChangesAsync();
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
                }
            }
        }
    }
}