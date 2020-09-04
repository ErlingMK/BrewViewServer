using System;
using System.Collections.Generic;
using System.Linq;
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
        public TestController()
        {
            
        }

        [GoogleAuthorizationHandler]
        [HttpGet]
        public IActionResult Get()
        {
            var user = HttpContext.User;

            return new OkObjectResult(new
            {
                user.Identity.Name
            });
        }
    }
}