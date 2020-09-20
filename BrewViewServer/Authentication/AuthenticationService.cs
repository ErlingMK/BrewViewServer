using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BrewViewServer.Authentication.Google;
using BrewViewServer.Repositories;
using BrewViewServer.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BrewViewServer.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IGoogleAuthentication m_googleAuthentication;
        private readonly IConfiguration m_configuration;
        private JwtSecurityTokenHandler m_tokenHandler;

        public AuthenticationService(IGoogleAuthentication googleAuthentication, IConfiguration configuration)
        {
            m_googleAuthentication = googleAuthentication;
            m_configuration = configuration;
        }

        public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var values)) return AuthenticateResult.Fail("Missing bearer token");

            var jwt = values.ToString().Split(' ').LastOrDefault();

            try
            {
                m_tokenHandler = new JwtSecurityTokenHandler();
                var token = m_tokenHandler.ReadJwtToken(jwt);
                if(token.Payload.Iss == null) return AuthenticateResult.Fail("Missing issuer");

                // TODO: Better way to do this? 
                if (scheme == "admin" && token.Subject != m_configuration["GoogleAuth:adminSub"]) return AuthenticateResult.Fail("no access");

                return token.Payload.Iss switch
                {
                    "https://accounts.google.com" => AuthenticateResult.Success(
                        new AuthenticationTicket(await ValidateGoogleToken(token, jwt, scheme), scheme)),
                    _ => AuthenticateResult.Fail("Unknown issuer")
                };
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail(e);
            }
        }

        private async Task<ClaimsPrincipal> ValidateGoogleToken(JwtSecurityToken token, string jwtAsString,
            string scheme)
        {
            var certificate = await m_googleAuthentication.GetCertificate(token.Header.Kid);

            var tokenValidationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false
            };

            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(
                new RSAParameters
                {
                    Modulus = Base64Util.FromBase64Url(certificate.N),
                    Exponent = Base64Util.FromBase64Url(certificate.E)
                });
            tokenValidationParameters.IssuerSigningKey = new RsaSecurityKey(rsa);

            return m_tokenHandler.ValidateToken(jwtAsString, tokenValidationParameters, out var validatedToken);
        }

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }
    }
}
