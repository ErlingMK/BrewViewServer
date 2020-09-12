using System.Threading.Tasks;
using BrewViewServer.Models;
using BrewViewServer.Repositories;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BrewViewServer.GraphQL
{
    public class Mutation
    {
        private readonly IBrewRepository m_brewRepository;
        private readonly IHttpContextAccessor m_contextAccessor;

        public Mutation([Service] IBrewRepository brewRepository, [Service] IHttpContextAccessor contextAccessor)
        {
            m_brewRepository = brewRepository;
            m_contextAccessor = contextAccessor;
        }

        public async Task<Brew> Create(Brew brew)
        {
            return await m_brewRepository.Create(brew);
        }

        public async Task<AppUserBrew> MakeFavorite(Brew brew)
        {
            return await m_brewRepository.Favorite(brew, m_contextAccessor.HttpContext.User);
        }

        public async Task<AppUserBrew> Rate(string productId, int rating)
        {
            return await m_brewRepository.Rate(productId, rating, m_contextAccessor.HttpContext.User);
        }

        public async Task<AppUserBrew> MakeNote(string productId, Note note)
        {
            return await m_brewRepository.MakeNote(productId, note, m_contextAccessor.HttpContext.User);
        }
    }
}