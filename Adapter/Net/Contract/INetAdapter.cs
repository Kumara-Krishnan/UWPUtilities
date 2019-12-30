using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace UWPUtilities.Adapter.Net.Contract
{
    public interface INetAdapter
    {
        void SetDefaultRequestHeaders(IDictionary<string, string> requestHeaders);

        Task<string> GetAsync(string uriString);

        Task<string> PostAsync(string uriString, IHttpContent content = default);

        Task<string> SendAsync(string uriString, HttpMethod httpMethod, IHttpContent content);

        Task<string> SendAsync(HttpRequestMessage requestMessage);
    }
}
