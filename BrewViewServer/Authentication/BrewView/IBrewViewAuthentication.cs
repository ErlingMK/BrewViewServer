using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BrewView.DatabaseModels.User;

namespace BrewView.Server.Authentication.BrewView
{
    public interface IBrewViewAuthentication
    {
        ClaimsPrincipal ValidateToken(JwtSecurityToken token, string jwtAsString);
        string CreateToken(User user);
    }
}