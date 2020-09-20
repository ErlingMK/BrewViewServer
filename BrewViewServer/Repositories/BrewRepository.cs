using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BrewViewServer.Models;
using BrewViewServer.Models.VinmonopolModels;
using BrewViewServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Repositories
{
    public class BrewRepository : IBrewRepository
    {
        private readonly BrewContext m_db;
        private readonly IUserService m_userService;

        public BrewRepository(BrewContext db, IUserService userService)
        {
            m_db = db;
            m_userService = userService;
        }

        public async Task<AlcoholicEntity> Get(string productId)
        {
            return await m_db.AlcoholicEntities.FindAsync(productId);
        }

        public async Task<Brew> GetBrew(string productId)
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

        public async Task<bool> Favorite(string id)
        {
            var brew = await m_db.Brews.FindAsync(id);
            var user = await m_db.Users.FindAsync();

            if (brew == null || user == null) return false;

            var appUserBrew = new UserBrew
            {
                UserId = user.Id,
                ProductId = brew.ProductId
            };

            m_db.Add(appUserBrew);
            await m_db.SaveChangesAsync();
            return true;
        }

        public async Task<UserBrew> Rate(string productId, int rating)
        {
            var appUserBrew = await m_db.UserBrews.FindAsync(productId, m_userService.GetUserName());

            if (appUserBrew != null)
            {
                appUserBrew.Rating = rating;
                await m_db.SaveChangesAsync();
            }

            return appUserBrew;
        }

        public async Task<UserBrew> MakeNote(string productId, Note note)
        {
            var appUserBrew = await m_db.UserBrews.FindAsync(productId, m_userService.GetUserName());

            if (appUserBrew != null)
            {
                appUserBrew.Notes.Add(note);
                await m_db.SaveChangesAsync();
            }

            return appUserBrew;
        }

        public async Task<Brew> GetBrewByGtin(string gtin)
        {
            return await m_db.Brews.SingleAsync(b => b.Gtin == gtin);
        }

        public async Task<AlcoholicEntity> GetByGtin(string gtin)
        {
            var brew = await m_db.Brews.SingleOrDefaultAsync(b => b.Gtin == gtin);

            if (brew == null)
            {
                return null;
            }

            return await m_db.AlcoholicEntities.FindAsync(brew.ProductId);
        }

        public async Task<IList<UserBrew>> GetBrews()
        {
            return await m_db.UserBrews.Where(b => b.UserId == m_userService.GetUserName())
                .Include(b => b.Notes).ToListAsync();
        }
    }

    public interface IBrewRepository
    {
        Task<AlcoholicEntity> Get(string productId);
        Task<Brew> GetBrew(string productId);
        Task<Brew> Create(Brew brew);
        Task<bool> Favorite(string id);
        Task<UserBrew> Rate(string productId, int rating);
        Task<UserBrew> MakeNote(string productId, Note note);
        Task<Brew> GetBrewByGtin(string gtin);
        Task<AlcoholicEntity> GetByGtin(string gtin);
        Task<IList<UserBrew>> GetBrews();
    }
}