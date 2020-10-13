using System.Threading.Tasks;
using BrewView.DatabaseModels.Models;
using BrewView.DatabaseModels.User;
using BrewView.Server.Models;

namespace BrewView.Server.Services.Abstractions
{
    public interface IOAuthService
    {
        Task<string> RedirectToAuthentication(AuthenticationProvider authenticationProvider, string codeChallenge,
            string state);
        Task<UserValidationResponse> RequestToken(string code, AuthenticationProvider authenticationProvider,
            string codeVerifier);
        Task<TokenResponse> RefreshToken(string refreshToken, AuthenticationProvider authenticationProvider);
    }
}