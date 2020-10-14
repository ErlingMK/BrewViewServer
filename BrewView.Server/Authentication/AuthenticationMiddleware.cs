using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BrewView.Server.Authentication
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
            if (ShouldAuthenticate(user))
            {
                if (await Authenticate(user, authenticationService))
                    await _next(user);
                else
                    user.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                await _next(user);
            }
        }

        private static bool ShouldAuthenticate(HttpContext user)
        {
            //TODO: Check target controller instead of string verifying.
            var path = user.Request.Path.ToUriComponent().ToLower();
            return !(path.Contains("auth") || path.Contains("playground") || path.Contains("error"));
        }

        private async Task<bool> Authenticate(HttpContext user, IAuthenticationService authenticationService)
        {
            var result = await authenticationService.AuthenticateAsync(user, string.Empty);

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