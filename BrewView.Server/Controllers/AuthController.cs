using System;
using System.Threading.Tasks;
using BrewView.Contracts;
using BrewView.Contracts.User;
using BrewView.Server.Models;
using BrewView.Server.Repositories.Abstractions;
using BrewView.Server.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BrewView.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> m_logger;
        private readonly IOAuthService m_oAuthService;
        private readonly IUserRepository m_userRepository;

        public AuthController(IUserRepository userRepository, IOAuthService oAuthService,
            ILogger<AuthController> logger)
        {
            m_userRepository = userRepository;
            m_oAuthService = oAuthService;
            m_logger = logger;
        }

        [HttpPost]
        [Route("user/create")]
        public async Task<IActionResult> Post([FromBody] CredentialsModel credentials)
        {
            m_logger.LogDebug($"Try create user {credentials.Email}");
            var response = await m_userRepository.Create(credentials.Email, credentials.Password);
            m_logger.LogDebug($"{response.Succeeded} {response.Message}");
            return Ok(response);
        }

        [HttpPost]
        [Route("user/signin")]
        public async Task<IActionResult> Get([FromBody] CredentialsModel credentialsModel)
        {
            m_logger.LogDebug($"Try sign-in user {credentialsModel.Email}");
            var response = await m_userRepository.SignIn(credentialsModel);
            m_logger.LogDebug($"{response.Succeeded} {response.Message}");

            return Ok(response);
        }

        [HttpGet]
        [Route("user/{provider}")]
        public async Task<IActionResult> Get([FromRoute] string provider, [FromQuery] string codeChallenge,
            [FromQuery] string state)
        {
            m_logger.LogDebug($"Open id connect sign-in with {provider}\nstate;{state}\nchallenge{codeChallenge}");
            return Redirect(
                await m_oAuthService.RedirectToAuthentication(Enum.Parse<AuthenticationProvider>(provider, true),
                    codeChallenge, state));
        }

        [HttpGet]   
        [Route("user/redirect/{provider}")]
        public async Task<IActionResult> GetTokens([FromRoute] string provider, [FromQuery] string code,
            [FromQuery] string codeVerifier)
        {
            m_logger.LogDebug($"Open id connect redirect with {provider}\ncode{code}\nverifier{codeVerifier}");

            var response = await m_oAuthService.RequestToken(code, Enum.Parse<AuthenticationProvider>(provider, true),
                codeVerifier);

            return Ok(response);
        }

        [HttpPost]
        [Route("user/refresh/{provider}")]
        public async Task<IActionResult> Post([FromBody] TokenResponse response, [FromRoute] string provider)
        {
            m_logger.LogDebug($"Open id connect refresh with {provider}");

            var token = await m_oAuthService.RefreshToken(response.RefreshToken,
                Enum.Parse<AuthenticationProvider>(provider, true));

            return Ok(token);
        }
    }
}