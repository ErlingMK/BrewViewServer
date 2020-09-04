using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BrewViewServer.Controllers;
using Newtonsoft.Json;

namespace BrewViewServer.Repositories
{
    public class GoogleRepository : IGoogleRepository
    {
        private static readonly string m_discoveryEndpoint =
            "https://accounts.google.com/.well-known/openid-configuration";

        private readonly HttpClient m_client;

        public GoogleRepository(IHttpClientFactory clientFactory)
        {
            m_client = clientFactory.CreateClient("GoogleAuth");
        }

        private GoogleAuthInfo GoogleAuthInfo { get; set; }
        private GoogleCertificates GoogleCertificates { get; set; }

        public async Task<string> GetAuthEndpoint()
        {
            await ReadDiscoveryEndpoint();

            return GoogleAuthInfo.AuthorizationEndpoint;
        }

        public async Task<string> GetTokenEndpoint()
        {
            await ReadDiscoveryEndpoint();

            return GoogleAuthInfo.TokenEndpoint;
        }

        public async Task<Key> GetCertificate(string kid)
        {
            await ReadDiscoveryEndpoint();

            if (GoogleCertificates.Keys.Any(key => key.Kid == kid))
            {
                return GoogleCertificates.Keys.First(key => key.Kid == kid);
            }

            var certResponse = await m_client.SendAsync(new HttpRequestMessage(HttpMethod.Get, GoogleAuthInfo.CertEndpoint));
            GoogleCertificates =  JsonConvert.DeserializeObject<GoogleCertificates>(await certResponse.Content.ReadAsStringAsync());

            return GoogleCertificates.Keys.Single(key => key.Kid == kid);
        }

        private async Task ReadDiscoveryEndpoint()
        {
            if (GoogleAuthInfo != null) return;

            var response = await m_client.SendAsync(new HttpRequestMessage(HttpMethod.Get, m_discoveryEndpoint));

            GoogleAuthInfo = JsonConvert.DeserializeObject<GoogleAuthInfo>(await response.Content.ReadAsStringAsync());
        }
    }

    internal class GoogleAuthInfo
    {
        [JsonProperty("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [JsonProperty("token_endpoint")]
        public string TokenEndpoint { get; set; }

        [JsonProperty("jwks_uri")] 
        public string CertEndpoint { get; set; }
    }

    public class GoogleCertificates
    {
        public IList<Key> Keys { get; set; }
    }

    public class Key
    {
        public string Kid { get; set; }
        public string N { get; set; }
        public string E { get; set; }
        public string Use { get; set; }
        public string Kty { get; set; }
        public string Alg { get; set; }
    }
}