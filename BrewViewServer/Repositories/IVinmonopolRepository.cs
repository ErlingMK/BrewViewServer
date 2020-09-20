using System;
using System.Threading.Tasks;

namespace BrewViewServer.Repositories
{
    public interface IVinmonopolRepository
    {
        Task<int> GetAllProducts();
        Task<int> GetProductsUpdatedSince(DateTime dateTime);
    }
}