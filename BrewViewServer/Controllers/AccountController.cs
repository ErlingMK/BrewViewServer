using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrewViewServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrewViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> m_userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            m_userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistrationModel registrationModel)
        {
            var identityResult = await m_userManager.CreateAsync(new AppUser() { UserName = registrationModel.Username}, registrationModel.Password);

            if (identityResult.Succeeded)
            {
                return Ok("Account created");
            }

            return BadRequest();
        }
    }
}