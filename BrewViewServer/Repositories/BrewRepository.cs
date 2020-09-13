using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BrewViewServer.Models;
using BrewViewServer.Models.VinmonopolModels;
using BrewViewServer.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BrewViewServer.Repositories
{
    public class BrewRepository : IBrewRepository
    {
        private readonly BrewContext m_db;
        private readonly HttpClient m_client;

        public BrewRepository(BrewContext db, IHttpClientFactory clientFactory)
        {
            m_db = db;
            m_client = clientFactory.CreateClient();
        }

        public async Task<Brew> Get(string productId)
        {
            return await m_db.Brews.FindAsync(productId);
        }

        public async Task<Brew> Create(Brew brew)
        {
            var entity = await m_db.Brews.FindAsync(brew.ProductId);

            if (entity == null)
            {
                entity = m_db.Brews.Add(brew).Entity;
                await m_db.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<UserBrew> Favorite(Brew brewInput, ClaimsPrincipal httpContextUser)
        {
            var brew = await m_db.Brews.FindAsync(brewInput.ProductId);
            var user = await m_db.Users.FindAsync(httpContextUser.Identity.Name);

            if (brew != null && user != null)
            {
                var appUserBrew = new UserBrew
                {
                    UserId = user.Id,
                    ProductId = brew.ProductId
                };
                m_db.Add(appUserBrew);
                await m_db.SaveChangesAsync();
                return appUserBrew;
            }

            return null;
        }

        public async Task<UserBrew> Rate(string productId, int rating, ClaimsPrincipal httpContextUser)
        {
            var appUserBrew = await m_db.UserBrews.FindAsync(productId, httpContextUser.Identity.Name);

            if (appUserBrew != null)
            {
                appUserBrew.Rating = rating;
                await m_db.SaveChangesAsync();
            }

            return appUserBrew;
        }

        public async Task<UserBrew> MakeNote(string productId, Note note, ClaimsPrincipal httpContextUser)
        {
            var appUserBrew = await m_db.UserBrews.FindAsync(productId, httpContextUser.Identity.Name);

            if (appUserBrew != null)
            {
                appUserBrew.Notes.Add(note);
                await m_db.SaveChangesAsync();
            }

            return appUserBrew;
        }

        public async Task<Brew> GetByGtin(string gtin)
        {
            return await m_db.Brews.SingleAsync(b => b.Gtin == gtin);
        }

        public async Task<IList<UserBrew>> GetBrews(IHttpContextAccessor contextAccessor)
        {
            return await m_db.UserBrews.Where(b => b.UserId == contextAccessor.HttpContext.User.Identity.Name)
                .Include(b => b.Notes).ToListAsync();
        }

        public async Task<bool> UpdateDatabase()
        {
            var count = 0;
            for (var i = 0; i < 10; i++)
                try
                {
                    var response = await m_client.SendAsync(new HttpRequestBuilder().WithMethod(HttpMethod.Get)
                        .WithRequestUri($"{AppConstants.ApiUrl}{AppConstants.ProductDetailsEndPoint}")
                        .AddHeader("Ocp-Apim-Subscription-Key", $"{AppConstants.ApiKey}")
                        .AddQueryParameter("start", count.ToString())
                        .Build());

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                        break; // Seems to be only way to know when all items are retrieved.

                    var content = await response.Content.ReadAsStringAsync();

                    var alcoholicEntities = JsonConvert.DeserializeObject<List<AlcoholicEntity>>(content);
                    foreach (var entity in alcoholicEntities)
                    {
                        entity.ProductId = entity.Basic.ProductId;
                        await WriteToDb(entity);
                    }

                    count += alcoholicEntities.Count;
                }
                catch (Exception e)
                {
                    return false;
                }

            return true;
        }

        private async Task WriteToDb(AlcoholicEntity entity)
        {
            m_db.AlcoholicEntities.Add(entity);
            m_db.Brews.Add(new Brew
            {
                Gtin = entity.Logistics.Barcodes.First(barcode => barcode.IsMainGtin).Gtin, ProductId = entity.ProductId
            });
            await m_db.SaveChangesAsync();
        }
    }

    public interface IBrewRepository
    {
        Task<Brew> Get(string productId);
        Task<Brew> Create(Brew brew);
        Task<UserBrew> Favorite(Brew brewInput, ClaimsPrincipal httpContextUser);
        Task<UserBrew> Rate(string productId, int rating, ClaimsPrincipal httpContextUser);
        Task<UserBrew> MakeNote(string productId, Note note, ClaimsPrincipal httpContextUser);
        Task<Brew> GetByGtin(string gtin);
        Task<IList<UserBrew>> GetBrews(IHttpContextAccessor contextAccessor);
        Task<bool> UpdateDatabase();
    }
}