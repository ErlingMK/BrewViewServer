using System;
using System.Threading.Tasks;
using BrewView.DatabaseModels.User;
using BrewView.Server.Models;
using BrewView.Server.Repositories.Abstractions;
using BrewView.Server.Services;
using BrewView.Server.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BrewView.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOAuthService m_oAuthService;
        private readonly IUserRepository m_userRepository;

        public AuthController(IUserRepository userRepository, IOAuthService oAuthService)
        {
            m_userRepository = userRepository;
            m_oAuthService = oAuthService;
        }

        [HttpPost]
        [Route("create/user")]
        public async Task<IActionResult> Post([FromBody] CredentialsModel credentials)
        {
            var result = await m_userRepository.Create(credentials.Email, credentials.Password);

            return result.Succeeded ? Ok(result) : Problem(result.Message);
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Get([FromBody] CredentialsModel credentialsModel)
        {
            var response = await m_userRepository.SignIn(credentialsModel);

            if (response.Succeeded) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet]
        [Route("{provider}")]
        public async Task<IActionResult> Get([FromRoute] string provider)
        {
            return Redirect(
                await m_oAuthService.RedirectToAuthentication(Enum.Parse<AuthenticationProvider>(provider, true)));
        }

        [HttpGet]
        [Route("redirect/{provider}")]
        public async Task<IActionResult> Get([FromRoute] string provider, string state, string code, string scope)
        {
            var token = await m_oAuthService.RequestToken(code, Enum.Parse<AuthenticationProvider>(provider, true));

            var response = await m_userRepository.CreateOAuthUser(token);

            if (response.Succeeded) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost]
        [Route("refresh/{provider}")]
        public async Task<IActionResult> Post([FromBody] TokenResponse response, [FromRoute] string provider)
        {
            var token = await m_oAuthService.RefreshToken(response.RefreshToken, Enum.Parse<AuthenticationProvider>(provider, true));

            return Ok(token);
        }
    }
}