using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace BrewViewServer.Controllers
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}