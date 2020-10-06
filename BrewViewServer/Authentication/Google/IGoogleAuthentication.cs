using System.Threading.Tasks;

namespace BrewView.Server.Authentication.Google
{
    public interface IGoogleAuthentication
    {
        Task<string> GetAuthEndpoint();
        Task<string> GetTokenEndpoint();
        Task<Key> GetCertificate(string kid);
    }
}