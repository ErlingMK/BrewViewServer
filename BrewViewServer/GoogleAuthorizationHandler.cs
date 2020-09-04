using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using BrewViewServer.Repositories;
using BrewViewServer.Util;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace BrewViewServer
{
    public class GoogleAuthorizationHandler : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var googleRepository = context.HttpContext.RequestServices.GetService(typeof(IGoogleRepository)) as IGoogleRepository;

            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var jwt)) return;

            var strings = jwt.ToString().Split(' ');

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(strings.LastOrDefault());

            var certificate = await googleRepository.GetCertificate(jwtSecurityToken.Header.Kid);

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

            handler.ValidateToken(jwt, tokenValidationParameters, out var validatedToken);

            await next();
        }
    }
}
