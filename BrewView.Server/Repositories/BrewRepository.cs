using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrewView.DatabaseModels;
using BrewView.DatabaseModels.Models;
using BrewView.DatabaseModels.Vinmonopol;
using BrewView.Server.Repositories.Abstractions;
using BrewView.Server.Services;
using BrewView.Server.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BrewView.Server.Repositories
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

        public async Task<UserBrew> GetUserBrew(string productId)
        {
            return await m_db.UserBrews.Include(brew => brew.Notes).FirstOrDefaultAsync(brew =>
                brew.ProductId == productId && brew.UserId == m_userService.CurrentUser);
        }

        public async Task<bool> Favorite(string id)
        {
            if (await GetUserBrew(id) != null) return true;

            var brew = await m_db.Brews.FindAsync(id);
            var user = await m_db.Users.FindAsync(m_userService.CurrentUser);

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

        public async Task<int?> UpdateDrunkCount(string productId, int count)
        {
            var userBrew = await GetUserBrew(productId);

            if (userBrew != null)
            {
                userBrew.DrunkCount = count;
                await m_db.SaveChangesAsync();
            }

            return userBrew?.DrunkCount;
        }

        public async Task<UserBrew> Rate(string productId, int rating)
        {
            var userBrew = await GetUserBrew(productId);

            if (userBrew != null)
            {
                userBrew.Rating = rating;
                await m_db.SaveChangesAsync();
            }

            return userBrew;
        }

        public async Task<UserBrew> MakeNote(string productId, Note note)
        {
            var userBrew = await GetUserBrew(productId);

            if (userBrew != null)
            {
                note.Date = DateTime.Now;
                userBrew.Notes.Add(note);
                await m_db.SaveChangesAsync();
            }

            return userBrew;
        }

        public async Task<Brew> GetBrewByGtin(string gtin)
        {
            return await m_db.Brews.SingleAsync(b => b.Gtin == gtin);
        }

        public async Task<AlcoholicEntity> GetByGtin(string gtin)
        {
            var brew = await m_db.Brews.SingleOrDefaultAsync(b => b.Gtin == gtin);

            if (brew == null) return null;

            return await Get(brew.ProductId);
        }

        public async Task<IList<AlcoholicEntity>> GetBrews()
        {
            var brews = await m_db.UserBrews.Where(b => b.UserId == m_userService.CurrentUser)
                .Select(brew => brew.ProductId).ToListAsync();
            return await m_db.AlcoholicEntities.Where(entity => brews.Contains(entity.ProductId)).ToListAsync();
        }

        public async Task<IList<UserBrew>> GetUserBrews()
        {
            return await m_db.UserBrews.Where(brew => brew.UserId == m_userService.CurrentUser).Include(brew => brew.Notes).ToListAsync();
        }
    }
}