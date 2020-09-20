using Newtonsoft.Json;

namespace BrewViewServer.Authentication.Google
{
    internal class GoogleAuthInfo
    {
        public bool IsEmpty => string.IsNullOrEmpty(AuthorizationEndpoint) || string.IsNullOrEmpty(TokenEndpoint) ||
                               string.IsNullOrEmpty(CertEndpoint);

        [JsonProperty("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [JsonProperty("token_endpoint")]
        public string TokenEndpoint { get; set; }

        [JsonProperty("jwks_uri")] 
        public string CertEndpoint { get; set; }
    }
}