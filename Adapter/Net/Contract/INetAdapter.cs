using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.Adapter.Net.Contract
{
    public interface INetAdapter
    {
        void SetDefaultRequestHeaders(IDictionary<string, string> requestHeaders);

        Task<string> GetAsync(string uriString);
    }
}
