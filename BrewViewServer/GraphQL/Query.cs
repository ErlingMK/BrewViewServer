using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using BrewViewServer.Models;
using BrewViewServer.Repositories;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;

namespace BrewViewServer.GraphQL
{
    public class Query
    {
        private readonly IBrewRepository m_repository;
        private readonly IHttpContextAccessor m_contextAccessor;

        public Query([Service] IBrewRepository repository, [Service] IHttpContextAccessor contextAccessor)
        {
            m_repository = repository;
            m_contextAccessor = contextAccessor;
        }
            
        public async Task<Brew> GetBrew(string productId)
        {
            return await m_repository.Get(productId);
        }

        public async Task<string> GetBrewId(string gtin)
        {
            var brew = await m_repository.GetByGtin(gtin);
            return brew.ProductId;
        }

        public async Task<IList<AppUserBrew>> GetBrews()
        {
            return await m_repository.GetBrews(m_contextAccessor);
        }
    }
}
