using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BrewViewServer.Models;
using BrewViewServer.Repositories;
using BrewViewServer.Services;
using BrewViewServer.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BrewViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory m_clientFactory;
        private readonly IConfiguration m_configuration;
        private readonly IGoogleRepository m_googleRepository;
        private readonly TokenService m_tokenService;
        private readonly UserManager<AppUser> m_userManager;

        public AuthController(UserManager<AppUser> userManager, TokenService tokenService,
            IHttpClientFactory clientFactory, IConfiguration configuration, IGoogleRepository googleRepository)
        {
            m_userManager = userManager;
            m_tokenService = tokenService;
            m_clientFactory = clientFactory;
            m_configuration = configuration;
            m_googleRepository = googleRepository;
        }

        [HttpGet]
        [Route("{scheme}")]
        public async Task<IActionResult> Get([FromRoute] string scheme)
        {
            //TODO: Verify this later
            var generateStateValue = StringGenerator.GenerateStateValue();

            var url = OAuthRequestBuilder.AppendQueryString(m_configuration["GoogleAuth:client_id"], m_configuration["GoogleAuth:redirect_uri"],
                new List<string> {"openid", "email"}, generateStateValue, await m_googleRepository.GetAuthEndpoint(),
                "arandomnoonce");

            return Redirect(url);
        }

        [HttpGet]
        [Route("redirect/g")]
        public async Task<IActionResult> Get(string state, string code, string scope)
        {
            var httpClient = m_clientFactory.CreateClient();

            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Post, await m_googleRepository.GetTokenEndpoint())
                {
                    Content = OAuthRequestBuilder.TokenRequestContent(code, m_configuration["GoogleAuth:client_id"],
                        m_configuration["GoogleAuth:client_secret"], m_configuration["GoogleAuth:redirect_uri"])
                };

            var response = await httpClient.SendAsync(httpRequestMessage);

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
            
            return Ok(tokenResponse.IdToken);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialsModel credentials)
        {
            var jwt = await VerifyUser(credentials.Username, credentials.Password);

            return new OkObjectResult(jwt);
        }

        private async Task<string> VerifyUser(string credentialsUsername, string credentialsPassword)
        {
            var user = await m_userManager.FindByNameAsync(credentialsUsername);

            var result = await m_userManager.CheckPasswordAsync(user, credentialsPassword);

            return result ? m_tokenService.CreateToken(user) : string.Empty;
        }
    }
}