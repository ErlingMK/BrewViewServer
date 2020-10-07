using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BrewView.Server.Authentication.Google;
using BrewView.Server.Models;
using BrewView.Server.Services.Abstractions;
using BrewView.Server.Util.Http;
using BrewView.Server.Util.StringUtils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BrewView.Server.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly HttpClient m_client;
        private readonly IConfiguration m_configuration;
        private readonly IGoogleAuthentication m_googleAuthentication;

        public OAuthService(IConfiguration configuration, IGoogleAuthentication googleAuthentication,
            IHttpClientFactory clientFactory)
        {
            m_configuration = configuration;
            m_googleAuthentication = googleAuthentication;
            m_client = clientFactory.CreateClient();
        }

        public async Task<string> RedirectToAuthentication(AuthenticationProvider authenticationProvider)
        {
            return await BuildAuthenticationRequest(authenticationProvider);
        }

        public async Task<TokenResponse> RequestToken(string code, AuthenticationProvider authenticationProvider)
        {
            var httpRequestMessage = await BuildTokenRequest(code, authenticationProvider);

            var response = await m_client.SendAsync(httpRequestMessage);
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TokenResponse>(json);
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

        private async Task<string> BuildAuthenticationRequest(AuthenticationProvider authenticationProvider)
        {
            var state = StringGenerator.GenerateStateValue(); //TODO: Verify this later

            return authenticationProvider switch
            {
                AuthenticationProvider.Google => OAuthRequestBuilder.AppendQueryString(
                    m_configuration["GoogleAuth:client_id"], m_configuration["GoogleAuth:redirect_uri"],
                    new List<string> {"openid", "email"}, state, await m_googleAuthentication.GetAuthEndpoint(),
                    "arandomnoonce"),
                _ => throw new ArgumentOutOfRangeException(nameof(authenticationProvider), authenticationProvider, null)
            };
        }

        private async Task<HttpRequestMessage> BuildTokenRequest(string code,
            AuthenticationProvider authenticationProvider)
        {
            return authenticationProvider switch
            {
                AuthenticationProvider.Google => new
                    HttpRequestMessage(HttpMethod.Post, await m_googleAuthentication.GetTokenEndpoint())
                    {
                        Content = OAuthRequestBuilder.TokenRequestContent(code, m_configuration["GoogleAuth:client_id"],
                            m_configuration["GoogleAuth:client_secret"], m_configuration["GoogleAuth:redirect_uri"])
                    },
                _ => throw new ArgumentOutOfRangeException(nameof(authenticationProvider), authenticationProvider, null)
            };
        }
    }
}