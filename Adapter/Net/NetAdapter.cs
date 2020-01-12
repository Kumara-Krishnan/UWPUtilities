using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Adapter.Net.Contract;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace UWPUtilities.Adapter.Net
{
    public sealed class NetAdapter : INetAdapter
    {
        private readonly HttpClient HttpClient;

        public NetAdapter()
        {
            HttpClient = new HttpClient();
        }

        public NetAdapter(IHttpFilter httpFilter)
        {
            HttpClient = new HttpClient(httpFilter);
        }

        public NetAdapter(IDictionary<string, string> requestHeaders) : this()
        {
            SetDefaultRequestHeaders(requestHeaders);
        }

        public NetAdapter(IHttpFilter httpFilter, IDictionary<string, string> requestHeaders) : this(httpFilter)
        {
            SetDefaultRequestHeaders(requestHeaders);
        }

        public void SetDefaultRequestHeaders(IDictionary<string, string> requestHeaders)
        {
            foreach (var requestHeader in requestHeaders)
            {
                HttpClient.DefaultRequestHeaders.Add(requestHeader.Key, requestHeader.Value);
            }
        }

        public Task<string> GetAsync(string uriString)
        {
            return SendAsync(uriString, HttpMethod.Get);
        }

        public Task<string> PostAsync(string uriString, IHttpContent content)
        {
            return SendAsync(uriString, HttpMethod.Post, content);
        }

        public Task<string> PostAsync(string uriString, IEnumerable<KeyValuePair<string, string>> requestParameters)
        {
            var content = new HttpFormUrlEncodedContent(requestParameters);
            return PostAsync(uriString, content);
        }

        public async Task<string> SendAsync(string uriString, HttpMethod httpMethod, IHttpContent content = default)
        {
            ValidateUri(uriString, out Uri uri);
            var request = new HttpRequestMessage(httpMethod, uri)
            {
                Content = content
            };
            var response = await HttpClient.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendAsync(HttpRequestMessage requestMessage)
        {
            var response = await HttpClient.SendRequestAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private void ValidateUri(string uriString, out Uri uri)
        {
            if (!Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out uri))
            {
                throw new ArgumentException("Invalid uri");
            }
        }
    }
}
