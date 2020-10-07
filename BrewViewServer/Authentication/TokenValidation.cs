using Microsoft.IdentityModel.Tokens;

namespace BrewView.Server.Authentication
{
    public class TokenValidation
    {
        public static TokenValidationParameters TokenValidationParameters =>
            new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false
            };
    }
}