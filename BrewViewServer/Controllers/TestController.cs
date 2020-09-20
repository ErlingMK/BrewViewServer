using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BrewViewServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrewViewServer.Controllers
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