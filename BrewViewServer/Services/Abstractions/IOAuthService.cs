using System.Threading.Tasks;
using BrewView.Server.Models;

namespace BrewView.Server.Services.Abstractions
{
    public interface IOAuthService
    {
        Task<string> RedirectToAuthentication(AuthenticationProvider authenticationProvider);
        Task<TokenResponse> RequestToken(string code, AuthenticationProvider authenticationProvider);
        Task<TokenResponse> RefreshToken(string refreshToken, AuthenticationProvider authenticationProvider);
    }
}