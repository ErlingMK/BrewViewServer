using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BrewView.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult Get()
        {
            var user = HttpContext.User;
            var id = user.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier);

            return new OkObjectResult(new
            {
                id.Value
            });
        }
    }
}