using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BrewView.Server.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error([FromServices] ILogger<ErrorController> logger)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            logger.LogError(context.Error, "");
            return Problem();
        }

        [Route("/error/dev")]
        public IActionResult ErrorDev([FromServices] ILogger<ErrorController> logger)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            logger.LogError(context.Error, "");
            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }
    }
}