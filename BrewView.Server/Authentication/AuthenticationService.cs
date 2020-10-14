using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BrewView.Server.Authentication.BrewView;
using BrewView.Server.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace BrewView.Server.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IBrewViewAuthentication m_brewViewAuthentication;
        private readonly IGoogleAuthentication m_googleAuthentication;
        private readonly JwtSecurityTokenHandler m_tokenHandler;

        public AuthenticationService(IGoogleAuthentication googleAuthentication,
            IBrewViewAuthentication brewViewAuthentication)
        {
            m_googleAuthentication = googleAuthentication;
            m_brewViewAuthentication = brewViewAuthentication;
            m_tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var values))
                return AuthenticateResult.Fail("Missing bearer token");

            var jwt = values.ToString().Split(' ').LastOrDefault();

            try
            {
                var token = m_tokenHandler.ReadJwtToken(jwt);
                if (token.Payload.Iss == null) return AuthenticateResult.Fail("Missing issuer");

                return token.Payload.Iss switch
                {
                    "https://accounts.google.com" => AuthenticateResult.Success(
                        new AuthenticationTicket(await ValidateGoogleToken(token, jwt), scheme)),
                    "https://brewview.com" => AuthenticateResult.Success(
                        new AuthenticationTicket(ValidateBrewViewToken(token, jwt), scheme)),
                    _ => AuthenticateResult.Fail("Unknown issuer")
                };
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail(e);
            }
        }

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal,
            AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        private ClaimsPrincipal ValidateBrewViewToken(JwtSecurityToken token, string jwt)
        {
            return m_brewViewAuthentication.ValidateToken(token, jwt);
        }

        private async Task<ClaimsPrincipal> ValidateGoogleToken(JwtSecurityToken token, string jwtAsString)
        {
            return await m_googleAuthentication.ValidateGoogleToken(token, jwtAsString);
        }
    }
}