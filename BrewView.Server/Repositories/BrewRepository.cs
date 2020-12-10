using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BrewView.DatabaseModels;
using BrewView.DatabaseModels.Models;
using BrewView.DatabaseModels.Vinmonopol;
using BrewView.Server.Repositories.Abstractions;
using BrewView.Server.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BrewView.Server.Repositories
{
    public class BrewRepository : IBrewRepository
    {
        private readonly BrewContext m_db;
        private readonly IUserService m_userService;
        private readonly IMapper m_mapper;

        public BrewRepository(BrewContext db, IUserService userService, IMapper mapper)
        {
            m_db = db;
            m_userService = userService;
            m_mapper = mapper;
        }

        public async Task<Contracts.Brew> Get(string productId)
        {
            var alcoholicEntity = await m_db.AlcoholicEntities.FindAsync(productId);
            alcoholicEntity.Basic.ProductId = alcoholicEntity.ProductId;
            return m_mapper.Map<Contracts.Brew>(alcoholicEntity);
        }

        public async Task<UserBrew> GetUserBrew(string productId)
        {
            return await m_db.UserBrews.Include(brew => brew.Notes).FirstOrDefaultAsync(brew =>
                brew.ProductId == productId && brew.UserId == m_userService.CurrentUser);
        }

        public async Task<bool> ToggleFavorite(string id)
        {
            var userBrew = await GetUserBrew(id);
            if (userBrew != null)
            {
                m_db.UserBrews.Remove(userBrew);
                await m_db.SaveChangesAsync();
                return true;
            };

            var brew = await m_db.ProductGtins.FindAsync(id);
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

        public async Task<Contracts.Brew> GetByGtin(string gtin)
        {
            var brew = await m_db.ProductGtins.SingleOrDefaultAsync(b => b.Gtin == gtin);

            if (brew == null) return null;

            return await Get(brew.ProductId);
        }

        public async Task<IList<Contracts.Brew>> GetBrews()
        {
            var brews = await m_db.UserBrews.Where(b => b.UserId == m_userService.CurrentUser)
                .Select(brew => brew.ProductId).ToListAsync();
            var alcoholicEntities = await m_db.AlcoholicEntities.Where(entity => brews.Contains(entity.ProductId)).ToListAsync();
            alcoholicEntities.ForEach(entity => entity.Basic.ProductId = entity.ProductId);
            return alcoholicEntities.Select(entity => m_mapper.Map<Contracts.Brew>(entity)).ToList();
        }

        public async Task<IList<UserBrew>> GetUserBrews()
        {
            return await m_db.UserBrews.Where(brew => brew.UserId == m_userService.CurrentUser)
                .Include(brew => brew.Notes).ToListAsync();
        }
    }
}