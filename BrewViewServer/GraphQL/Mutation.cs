﻿using System.Threading.Tasks;
using BrewViewServer.Models;
using BrewViewServer.Repositories;
using HotChocolate;

namespace BrewViewServer.GraphQL
{
    public class Mutation
    {
        private readonly IBrewRepository m_brewRepository;

        public Mutation([Service] IBrewRepository brewRepository)
        {
            m_brewRepository = brewRepository;
        }

        public async Task<bool> MakeFavorite(string productId)
        {
            return await m_brewRepository.Favorite(productId);
        }

        public async Task<UserBrew> Rate(string productId, int rating)
        {
            return await m_brewRepository.Rate(productId, rating);
        }

        public async Task<UserBrew> MakeNote(string productId, Note note)
        {
            return await m_brewRepository.MakeNote(productId, note);
        }

        public async Task<int?> UpdateDrunkenCount(string productId, int count)
        {
            return await m_brewRepository.UpdateDrunkCount(productId, count);
        }
    }
}