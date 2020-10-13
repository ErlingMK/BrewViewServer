using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BrewView.DatabaseModels.Models;
using BrewView.DatabaseModels.User;
using BrewView.Server.Authentication.Google;
using BrewView.Server.Models;
using BrewView.Server.Repositories.Abstractions;
using BrewView.Server.Services.Abstractions;
using BrewView.Server.Util.Http;
using BrewView.Server.Util.StringUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BrewView.Server.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly HttpClient m_client;
        private readonly IConfiguration m_configuration;
        private readonly IGoogleAuthentication m_googleAuthentication;
        private readonly ILogger<OAuthService> m_logger;
        private readonly IUserRepository m_userRepository;

        public OAuthService(IConfiguration configuration, IGoogleAuthentication googleAuthentication,
            IHttpClientFactory clientFactory, ILogger<OAuthService> logger, IUserRepository userRepository)
        {
            m_configuration = configuration;
            m_googleAuthentication = googleAuthentication;
            m_logger = logger;
            m_userRepository = userRepository;
            m_client = clientFactory.CreateClient();
        }

        public async Task<string> RedirectToAuthentication(AuthenticationProvider authenticationProvider,
            string codeChallenge, string state)
        {
            return await BuildAuthenticationRequest(authenticationProvider, codeChallenge, state);
        }

        public async Task<UserValidationResponse> RequestToken(string code, AuthenticationProvider authenticationProvider,
            string codeVerifier)
        {
            try
            {
                var httpRequestMessage = await BuildTokenRequest(code, authenticationProvider, codeVerifier);

                var responseMessage = await m_client.SendAsync(httpRequestMessage);
                var json = await responseMessage.Content.ReadAsStringAsync();

                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
                if (string.IsNullOrEmpty(tokenResponse.IdToken)) return new UserValidationResponse(false);

                return await m_userRepository.CreateOAuthUser(tokenResponse);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "unable to exchange code for token");
                return new UserValidationResponse(false);
            }
        }

        public async Task<TokenResponse> RefreshToken(string refreshToken, AuthenticationProvider authenticationProvider)
        {
            var httpRequestMessage = await BuildRefreshRequest(refreshToken, authenticationProvider);

            var response = await m_client.SendAsync(httpRequestMessage);
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TokenResponse>(json);
        }

        private async Task<HttpRequestMessage> BuildRefreshRequest(string refreshToken, AuthenticationProvider authenticationProvider)
        {
            return authenticationProvider switch
            {
                AuthenticationProvider.Google => new
                    HttpRequestMessage(HttpMethod.Post, await m_googleAuthentication.GetTokenEndpoint())
                    {
                        Content = OAuthRequestBuilder.RefreshRequestContent(refreshToken, m_configuration["GoogleAuth:client_id"],
                            m_configuration["GoogleAuth:client_secret"])
                    },
                _ => throw new ArgumentOutOfRangeException(nameof(authenticationProvider), authenticationProvider, null)
            };

        }

        private async Task<string> BuildAuthenticationRequest(AuthenticationProvider authenticationProvider,
            string codeChallenge, string state)
        {
            return authenticationProvider switch
            {
                AuthenticationProvider.Google => OAuthRequestBuilder.AppendQueryString(
                    m_configuration["GoogleAuth:client_id"], m_configuration["GoogleAuth:redirect_uri"],
                    new List<string> {"openid", "email"}, state, await m_googleAuthentication.GetAuthEndpoint(),
                    "arandomnoonce", codeChallenge),
                _ => throw new ArgumentOutOfRangeException(nameof(authenticationProvider), authenticationProvider, null)
            };
        }

        private async Task<HttpRequestMessage> BuildTokenRequest(string code,
            AuthenticationProvider authenticationProvider, string codeVerifier)
        {
            return authenticationProvider switch
            {
                AuthenticationProvider.Google => new
                    HttpRequestMessage(HttpMethod.Post, await m_googleAuthentication.GetTokenEndpoint())
                    {
                        Content = OAuthRequestBuilder.TokenRequestContent(code, m_configuration["GoogleAuth:client_id"],
                            codeVerifier, m_configuration["GoogleAuth:redirect_uri"])
                    },
                _ => throw new ArgumentOutOfRangeException(nameof(authenticationProvider), authenticationProvider, null)
            };
        }
    }
}