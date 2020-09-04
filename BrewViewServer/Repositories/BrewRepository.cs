using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BrewViewServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BrewViewServer.Repositories
{
    public class BrewRepository : IBrewRepository
    {
        private readonly BrewContext m_db;

        public BrewRepository(BrewContext db)
        {
            m_db = db;
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

        public async Task<AppUserBrew> Favorite(Brew brewInput, ClaimsPrincipal httpContextUser)
        {
            var brew = await m_db.Brews.FindAsync(brewInput.ProductId);
            var user = await m_db.Users.FindAsync(httpContextUser.Identity.Name);

            if (brew != null && user != null)
            {
                var appUserBrew = new AppUserBrew
                {
                    AppUserId = user.Id,
                    ProductId = brew.ProductId
                };
                m_db.Add(appUserBrew);
                await m_db.SaveChangesAsync();
                return appUserBrew;
            }

            return null;
        }

        public async Task<AppUserBrew> Rate(string productId, int rating, ClaimsPrincipal httpContextUser)
        {
            var appUserBrew = await m_db.AppUserBrews.FindAsync(productId, httpContextUser.Identity.Name);

            if (appUserBrew != null)
            {
                appUserBrew.Rating = rating;
                await m_db.SaveChangesAsync();
            }

            return appUserBrew;
        }

        public async Task<AppUserBrew> MakeNote(string productId, Note note, ClaimsPrincipal httpContextUser)
        {
            var appUserBrew = await m_db.AppUserBrews.FindAsync(productId, httpContextUser.Identity.Name);

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

        public async Task<IList<AppUserBrew>> GetBrews(IHttpContextAccessor contextAccessor)
        {
            return await m_db.AppUserBrews.Where(b => b.AppUserId == contextAccessor.HttpContext.User.Identity.Name)
                .Include(b => b.Notes).ToListAsync();
        }
    }

    public interface IBrewRepository
    {
        Task<Brew> Get(string productId);
        Task<Brew> Create(Brew brew);
        Task<AppUserBrew> Favorite(Brew brewInput, ClaimsPrincipal httpContextUser);
        Task<AppUserBrew> Rate(string productId, int rating, ClaimsPrincipal httpContextUser);
        Task<AppUserBrew> MakeNote(string productId, Note note, ClaimsPrincipal httpContextUser);
        Task<Brew> GetByGtin(string gtin);
        Task<IList<AppUserBrew>> GetBrews(IHttpContextAccessor contextAccessor);
    }
}