using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BrewView.Server.Authentication.Google
{
    public interface IGoogleAuthentication
    {
        Task<string> GetAuthEndpoint();
        Task<string> GetTokenEndpoint();
        Task<Key> GetCertificate(string kid);
        Task<ClaimsPrincipal> ValidateGoogleToken(JwtSecurityToken token, string jwtAsString);
    }
}