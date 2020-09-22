﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BrewViewServer.Models;
using BrewViewServer.Models.VinmonopolModels;

namespace BrewViewServer.Repositories
{
    public interface IBrewRepository
    {
        Task<AlcoholicEntity> Get(string productId);
        Task<Brew> GetBrew(string productId);
        Task<UserBrew> GetUserBrew(string productId);
        Task<bool> Favorite(string id);
        Task<int?> UpdateDrunkCount(string productId, int count);
        Task<UserBrew> Rate(string productId, int rating);
        Task<UserBrew> MakeNote(string productId, Note note);
        Task<Brew> GetBrewByGtin(string gtin);
        Task<AlcoholicEntity> GetByGtin(string gtin);
        Task<IList<AlcoholicEntity>> GetBrews();
        Task<IList<UserBrew>> GetUserBrews();
    }
}