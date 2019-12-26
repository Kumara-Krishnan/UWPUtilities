using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.BackgroundTransfer.Contract;
using UWPUtilities.BackgroundTransfer.Upload.Contract;
using Windows.Networking.BackgroundTransfer;
using Windows.Web.Http;

namespace UWPUtilities.BackgroundTransfer.Upload
{
    public class BackgroundUploadRequest : IBackgroundUploadRequest
    {
        public BackgroundUploadRequest()
        {

        }

        public HttpMethod Method => throw new NotImplementedException();

        public BackgroundTransferPriority Priority => throw new NotImplementedException();

        public BackgroundTransferGroup TransferGroup => throw new NotImplementedException();

        public BackgroundTransferCompletionGroup CompletionGroup => throw new NotImplementedException();

        public Uri RequestUri => throw new NotImplementedException();
    }
}
