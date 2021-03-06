﻿using Newtonsoft.Json;

namespace BrewView.Server.Authentication.Google
{
    public class GoogleAuthInfo
    {
        public bool IsEmpty => string.IsNullOrEmpty(AuthorizationEndpoint) || string.IsNullOrEmpty(TokenEndpoint) ||
                               string.IsNullOrEmpty(CertEndpoint);

        [JsonProperty("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [JsonProperty("token_endpoint")] public string TokenEndpoint { get; set; }

        [JsonProperty("jwks_uri")] public string CertEndpoint { get; set; }

        public string Issuer { get; set; }
    }
}