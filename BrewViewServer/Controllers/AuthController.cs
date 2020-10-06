using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BrewView.Server.Authentication.Google;
using BrewView.Server.Repositories;
using BrewView.Server.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BrewView.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory m_clientFactory;
        private readonly IConfiguration m_configuration;
        private readonly IGoogleAuthentication m_googleAuthentication;
        private readonly IUserRepository m_userRepository;

        public AuthController(IHttpClientFactory clientFactory, IConfiguration configuration, IGoogleAuthentication googleAuthentication, IUserRepository userRepository)
        {
            m_clientFactory = clientFactory;
            m_configuration = configuration;
            m_googleAuthentication = googleAuthentication;
            m_userRepository = userRepository;
        }

        [HttpGet]
        [Route("{scheme}")]
        public async Task<IActionResult> Get([FromRoute] string scheme)
        {
            //TODO: Verify this later
            var generateStateValue = StringGenerator.GenerateStateValue();
            var url = OAuthRequestBuilder.AppendQueryString(m_configuration["GoogleAuth:client_id"],
                m_configuration["GoogleAuth:redirect_uri"],
                new List<string> {"openid", "email"}, generateStateValue, await m_googleAuthentication.GetAuthEndpoint(),
                "arandomnoonce");

            return Redirect(url);
        }

        [HttpGet]
        [Route("redirect/g")]
        public async Task<IActionResult> Get(string state, string code, string scope)
        {
            var httpClient = m_clientFactory.CreateClient();

            var httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Post, await m_googleAuthentication.GetTokenEndpoint())
                {
                    Content = OAuthRequestBuilder.TokenRequestContent(code, m_configuration["GoogleAuth:client_id"],
                        m_configuration["GoogleAuth:client_secret"], m_configuration["GoogleAuth:redirect_uri"])
                };

            var response = await httpClient.SendAsync(httpRequestMessage);
            var json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(json);
            
            await m_userRepository.Create(token);   

            return Ok(token.IdToken);
        }
    }
}