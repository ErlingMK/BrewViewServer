using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using BrewView.Server.Util.StringUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BrewView.Server.Authentication.Google
{
    public class GoogleAuthentication : IGoogleAuthentication
    {
        private static readonly string m_discoveryEndpoint =
            "https://accounts.google.com/.well-known/openid-configuration";

        private readonly HttpClient m_client;
        private readonly IConfiguration m_configuration;
        private readonly JwtSecurityTokenHandler m_tokenHandler;
        private Timer m_timer;

        public GoogleAuthentication(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            m_configuration = configuration;
            m_tokenHandler = new JwtSecurityTokenHandler();
            m_client = clientFactory.CreateClient("GoogleAuth");

            m_timer = new Timer(TimerCallback, null, 0, int.Parse(m_configuration["GoogleAuth:renewCerts_timespan"]));
        }

        private GoogleAuthInfo GoogleAuthInfo { get; set; } = new GoogleAuthInfo();
        private GoogleCertificates GoogleCertificates { get; set; } = new GoogleCertificates();

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
                return GoogleCertificates.Keys.First(key => key.Kid == kid);

            await GetCerts();

            return GoogleCertificates.Keys.SingleOrDefault(key => key.Kid == kid);
        }

        public async Task<ClaimsPrincipal> ValidateGoogleToken(JwtSecurityToken token, string jwtAsString)
        {
            var certificate = await GetCertificate(token.Header.Kid);

            if (certificate == null) throw new SecurityTokenException("Missing certificates");

            var tokenValidationParameters = TokenValidation.TokenValidationParameters;

            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(
                new RSAParameters
                {
                    Modulus = Base64Util.FromBase64Url(certificate.N),
                    Exponent = Base64Util.FromBase64Url(certificate.E)
                });
            tokenValidationParameters.IssuerSigningKey = new RsaSecurityKey(rsa);

            return m_tokenHandler.ValidateToken(jwtAsString, tokenValidationParameters, out var validatedToken);
        }

        private async void TimerCallback(object state)
        {
            await ReadDiscoveryEndpoint(true);
            await GetCerts();
        }

        private async Task GetCerts()
        {
            var certResponse =
                await m_client.SendAsync(new HttpRequestMessage(HttpMethod.Get, GoogleAuthInfo.CertEndpoint));
            GoogleCertificates =
                JsonConvert.DeserializeObject<GoogleCertificates>(await certResponse.Content.ReadAsStringAsync());
        }

        private async Task ReadDiscoveryEndpoint(bool force = false)
        {
            if (!GoogleAuthInfo.IsEmpty && !force) return;

            var response = await m_client.SendAsync(new HttpRequestMessage(HttpMethod.Get, m_discoveryEndpoint));

            GoogleAuthInfo = JsonConvert.DeserializeObject<GoogleAuthInfo>(await response.Content.ReadAsStringAsync());
        }
    }
}