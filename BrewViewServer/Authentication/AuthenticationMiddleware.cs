using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BrewViewServer.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace BrewViewServer.Authentication
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext user, IAuthenticationService authenticationService)
        {
            if (user.Request.Path.ToUriComponent().Contains("auth"))
            {
                await _next(user);
            }
            else
            {
                if (await Authenticate(user, authenticationService))
                {
                    await _next(user);
                }
                user.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
        }

        private async Task<bool> Authenticate(HttpContext user, IAuthenticationService authenticationService)
        {
            var result = await authenticationService.AuthenticateAsync(user, "Bearer");

            if (result.Succeeded)
            {
                user.User = result.Principal;
                return true;
            }

            return false;
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthentication(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}