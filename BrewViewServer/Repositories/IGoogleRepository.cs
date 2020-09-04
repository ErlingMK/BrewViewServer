using System.Threading.Tasks;

namespace BrewViewServer.Repositories
{
    public interface IGoogleRepository
    {
        Task<string> GetAuthEndpoint();
        Task<string> GetTokenEndpoint();
        Task<Key> GetCertificate(string kid);
    }
}