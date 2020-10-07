using System.Threading.Tasks;

namespace VinmonopolQuery.Services
{
    public interface IVinmonopolService
    {
        Task<int> GetProducts(bool getAll = false, string dateTime = default);
    }
}