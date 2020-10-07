using Newtonsoft.Json;

namespace BrewView.Server.Models
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}