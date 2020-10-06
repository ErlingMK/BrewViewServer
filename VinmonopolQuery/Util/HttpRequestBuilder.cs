using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VinmonopolQuery.Util
{
    public class HttpRequestBuilder
    {
        private const string DefaultAcceptHeader = "application/json";

        private string m_requestUri = string.Empty;

        private readonly HttpRequestMessage m_requestMessage = new HttpRequestMessage();

        private readonly Dictionary<string, string?> m_queryParameters = new Dictionary<string, string?>();

        public HttpRequestBuilder WithMethod(HttpMethod method)
        {
            m_requestMessage.Method = method;
            return this;
        }

        public HttpRequestBuilder WithRequestUri(string requestUri)
        {
            this.m_requestUri = requestUri;
            return this;
        }

        public HttpRequestBuilder AddQueryParameter(string name, object value)
        {
            m_queryParameters[name] = value.ToString();
            return this;
        }

        public HttpRequestBuilder AddHeader(string name, string value)
        {
            m_requestMessage.Headers.Add(name, value);
            return this;
        }

        public HttpRequestBuilder WithAcceptHeader(string acceptHeader)
        {
            m_requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeader));
            return this;
        }

        public HttpRequestBuilder WithJsonContent<T>(T content) where T : class
        {
            m_requestMessage.Content = new JsonContent(content);
            return this;
        }

        public HttpRequestMessage Build()
        {
            var queryString = BuildQueryString();
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                m_requestUri = $"{m_requestUri}?{queryString}";
            }

            if (m_requestMessage.Headers.Accept.Count == 0)
            {
                WithAcceptHeader(DefaultAcceptHeader);
            }

            m_requestMessage.RequestUri = new Uri(m_requestUri);
            return m_requestMessage;
        }

        private string BuildQueryString()
        {
            if (m_queryParameters.Count == 0)
            {
                return string.Empty;
            }
            var encoder = UrlEncoder.Default;
            return m_queryParameters
                .Select(kvp => $"{encoder.Encode(kvp.Key)}={encoder.Encode(kvp.Value)}")
                .Aggregate((current, next) => $"{current}&{next}");
        }
    }

    public class JsonContent : StringContent
    {
        public JsonContent(object value)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8,
            "application/json")
        {
        }
    }

    public static class HttpResponseMessageResultExtensions
    {
        public static async Task<T> ContentAs<T>(this HttpResponseMessage response) where T : class
        {
            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
