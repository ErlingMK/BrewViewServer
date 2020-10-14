using System;
using System.Threading.Tasks;
using BrewView.Contracts;
using BrewView.Contracts.User;
using BrewView.Server.Models;
using BrewView.Server.Repositories.Abstractions;
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
        [Route("user/create")]
        public async Task<IActionResult> Post([FromBody] CredentialsModel credentials)
        {
            var response = await m_userRepository.Create(credentials.Email, credentials.Password);

            return Ok(response);
        }

        [HttpPost]
        [Route("user/signin")]
        public async Task<IActionResult> Get([FromBody] CredentialsModel credentialsModel)
        {
            var response = await m_userRepository.SignIn(credentialsModel);

            return Ok(response);
        }

        [HttpGet]
        [Route("user/{provider}")]
        public async Task<IActionResult> Get([FromRoute] string provider, [FromQuery] string codeChallenge,
            [FromQuery] string state)
        {
            return Redirect(
                await m_oAuthService.RedirectToAuthentication(Enum.Parse<AuthenticationProvider>(provider, true),
                    codeChallenge, state));
        }

        [HttpGet]
        [Route("user/redirect/{provider}")]
        public async Task<IActionResult> GetTokens([FromRoute] string provider, [FromQuery] string code,
            [FromQuery] string codeVerifier)
        {
            var response = await m_oAuthService.RequestToken(code, Enum.Parse<AuthenticationProvider>(provider, true),
                codeVerifier);

            return Ok(response);
        }

        [HttpPost]
        [Route("user/refresh/{provider}")]
        public async Task<IActionResult> Post([FromBody] TokenResponse response, [FromRoute] string provider)
        {
            var token = await m_oAuthService.RefreshToken(response.RefreshToken,
                Enum.Parse<AuthenticationProvider>(provider, true));

            return Ok(token);
        }
    }
}