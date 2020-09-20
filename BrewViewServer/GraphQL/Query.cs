using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using BrewViewServer.Models;
using BrewViewServer.Models.VinmonopolModels;
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

        public Query([Service] IBrewRepository repository)
        {
            m_repository = repository;
        }
            
        public async Task<AlcoholicEntity> GetBrew(string productId)
        {
            return await m_repository.Get(productId);
        }

        public async Task<AlcoholicEntity> GetBrewId(string gtin)
        {
            return await m_repository.GetByGtin(gtin);
        }

        public async Task<IList<UserBrew>> GetBrews()
        {
            return await m_repository.GetBrews();
        }
    }
}
