using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.BackgroundTransfer.Contract;

namespace UWPUtilities.BackgroundTransfer.Upload.Contract
{
    public interface IBackgroundUploadHandler<out TRequest, out TResponse> : IBackgroundTransferHandler<TRequest, TResponse>
        where TRequest : IBackgroundUploadRequest where TResponse : IBackgroundUploadResponse
    {

    }
}
