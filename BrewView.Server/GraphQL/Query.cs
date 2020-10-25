using System.Collections.Generic;
using System.Threading.Tasks;
using BrewView.DatabaseModels.Models;
using BrewView.DatabaseModels.Vinmonopol;
using BrewView.Server.Repositories;
using BrewView.Server.Repositories.Abstractions;
using HotChocolate;

namespace BrewView.Server.GraphQL
{
    public class Query
    {
        private readonly IBrewRepository m_repository;

        public Query([Service] IBrewRepository repository)
        {
            m_repository = repository;
        }
            
        public async Task<Contracts.Brew> GetBrew(string productId)
        {
            return await m_repository.Get(productId);
        }

        public async Task<Contracts.Brew> GetBrewId(string gtin)
        {
            return await m_repository.GetByGtin(gtin);
        }

        public async Task<IList<Contracts.Brew>> GetBrews()
        {
            return await m_repository.GetBrews();
        }

        public async Task<UserBrew> GetUserBrew(string id)
        {
            return await m_repository.GetUserBrew(id);
        }

        public async Task<IList<UserBrew>> GetUserBrews()
        {
            return await m_repository.GetUserBrews();
        }
    }
}
