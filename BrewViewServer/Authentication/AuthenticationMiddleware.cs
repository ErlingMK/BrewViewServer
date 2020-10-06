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
            var path = user.Request.Path.ToUriComponent();
            if (path.Contains("auth") || path.Contains("playground"))
            {
                await _next(user);
            }
            else if (path.Contains("vinmonopol"))
            {
                if (await Authenticate(user, authenticationService, "admin"))
                    await _next(user);
                else
                    user.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                if (await Authenticate(user, authenticationService))
                    await _next(user);
                else
                    user.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
        }

        private async Task<bool> Authenticate(HttpContext user, IAuthenticationService authenticationService, string scheme = "Bearer")
        {
            var result = await authenticationService.AuthenticateAsync(user, scheme);

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