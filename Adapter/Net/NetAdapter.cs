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

        public async Task<string> GetAsync(string uriString)
        {
            ValidateUri(uriString, out Uri uri);
            var response = await HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string uriString, IHttpContent content)
        {
            ValidateUri(uriString, out Uri uri);
            var response = await HttpClient.PostAsync(uri, content);
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
