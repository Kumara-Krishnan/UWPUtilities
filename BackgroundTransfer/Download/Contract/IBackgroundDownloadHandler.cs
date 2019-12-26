using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.BackgroundTransfer.Contract;
using UWPUtilities.BackgroundTransfer.Download;
using UWPUtilities.BackgroundTransfer.Download.Contract;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace UWPUtilities.BackgroundTransfer.Download.Contract
{
    public interface IBackgroundDownloadHandler<out TRequest, out TResponse> : IBackgroundTransferHandler<TRequest, TResponse>
        where TRequest : IBackgroundDownloadRequest where TResponse : IBackgroundDownloadResponse
    {
        Task<StorageFile> CreateTemporaryFileAsync();

        Task<DownloadOperation> CreateDownloadAsync();
    }
}
