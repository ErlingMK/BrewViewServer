using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace BrewViewServer.Util
{
    public class OAuthRequestBuilder
    {
        public static string AppendQueryString(string id, string redirectUri, IEnumerable<string> scopes,
            string state, string url, string nonce)
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
                {"nonce", nonce}

            };
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
    }
}
