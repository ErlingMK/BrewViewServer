using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace BrewView.Server.Util.Http
{
    public class OAuthRequestBuilder
    {
        public static string AppendQueryString(string id, string redirectUri, IEnumerable<string> scopes,
            string state, string url, string nonce, bool requestRefresh = true)
        {
            var scopeString = "";
            foreach (var scope in scopes)
            {
                scopeString += scope;
                scopeString += " ";
            }

            var trimmed = scopeString.Trim();

            var dict = new Dictionary<string, string>()
            {
                {"client_id", id},
                {"redirect_uri", redirectUri},
                {"scope", trimmed},
                {"response_type", "code"},
                {"state", state},
                {"nonce", nonce},
                {"prompt", "consent"}
            };

            if (requestRefresh) dict.Add("access_type", "offline");

            return QueryHelpers.AddQueryString(url, dict);
        }

        public static FormUrlEncodedContent TokenRequestContent(string code, string id, string secret, string redirectUri)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", id),
                new KeyValuePair<string, string>("client_secret", secret),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });
            return content;
        }

        public static FormUrlEncodedContent RefreshRequestContent(string refreshToken, string id, string secret)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("client_id", id),
                new KeyValuePair<string, string>("client_secret", secret),
                new KeyValuePair<string, string>("grant_type", "refresh_token")
            });
            return content;
        }
    }
}
