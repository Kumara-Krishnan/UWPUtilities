using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.BackgroundTransfer.Contract;
using UWPUtilities.BackgroundTransfer.Download.Contract;
using Windows.Networking.BackgroundTransfer;
using Windows.Web.Http;

namespace UWPUtilities.BackgroundTransfer.Download
{
    public class BackgroundDownloadRequest : IBackgroundDownloadRequest
    {
        public Uri RequestUri { get; private set; }

        public HttpMethod Method { get; private set; }

        public BackgroundTransferPriority Priority { get; set; }

        public BackgroundTransferGroup TransferGroup { get; private set; }

        public BackgroundTransferCompletionGroup CompletionGroup { get; private set; }

        public Dictionary<string, string> RequestHeaders { get; private set; }

        public BackgroundDownloadRequest(string requestUri, HttpMethod httpMethod)
        {
            if (!Uri.TryCreate(requestUri, UriKind.RelativeOrAbsolute, out Uri uri))
            {
                throw new ArgumentException("Invalid requestUri");
            }
            RequestUri = uri;
            Method = httpMethod;
        }

        public BackgroundDownloadRequest(string requestUri, HttpMethod httpMethod, BackgroundTransferGroup transferGroup) : this(requestUri, httpMethod)
        {
            TransferGroup = transferGroup;
        }

        public BackgroundDownloadRequest(string requestUri, HttpMethod httpMethod, BackgroundTransferCompletionGroup completionGroup) : this(requestUri, httpMethod)
        {
            CompletionGroup = completionGroup;
        }

        public BackgroundDownloadRequest(string requestUri, HttpMethod httpMethod, Dictionary<string, string> requestHeaders) : this(requestUri, httpMethod)
        {
            RequestHeaders = requestHeaders;
        }

        public BackgroundDownloadRequest(string requestUri, HttpMethod httpMethod, BackgroundTransferGroup transferGroup,
            Dictionary<string, string> requestHeaders) : this(requestUri, httpMethod, transferGroup)
        {
            RequestHeaders = requestHeaders;
        }
    }
}
