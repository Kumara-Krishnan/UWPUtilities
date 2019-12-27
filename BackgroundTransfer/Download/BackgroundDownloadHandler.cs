using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.BackgroundTransfer.Contract;
using UWPUtilities.BackgroundTransfer.Download.Contract;
using UWPUtilities.Extension;
using UWPUtilities.Util;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace UWPUtilities.BackgroundTransfer.Download
{
    public class BackgroundDownloadHandler : IBackgroundDownloadHandler<BackgroundDownloadRequest, BackgroundDownloadResponse>
    {
        public BackgroundDownloadRequest Request { get; set; }

        public BackgroundDownloadResponse Response { get; set; }

        public BackgroundDownloadHandler()
        {

        }

        public virtual Task<StorageFile> CreateTemporaryFileAsync()
        {
            var temporaryFolder = FileSystemUtil.GetApplicationFolder(ApplicationFolderType.Temporary);
            return temporaryFolder.CreateFileAsync(Guid.NewGuid().ToString(), CreationCollisionOption.GenerateUniqueName).AsTask();
        }

        public async Task<DownloadOperation> CreateDownloadAsync()
        {
            var downloader = Request.CompletionGroup is null ? new BackgroundDownloader() : new BackgroundDownloader(Request.CompletionGroup)
            {
                Method = Request.Method.ToString()
            };

            if (Request.TransferGroup != default)
            {
                downloader.TransferGroup = Request.TransferGroup;
            }

            if (Request.RequestHeaders.IsNonEmpty())
            {
                foreach (var requestHeader in Request.RequestHeaders)
                {
                    downloader.SetRequestHeader(requestHeader.Key, requestHeader.Value);
                }
            }
            var tempFile = await CreateTemporaryFileAsync().ConfigureAwait(false);
            DownloadOperation download = downloader.CreateDownload(Request.RequestUri, tempFile);
            download.Priority = Request.Priority;
            return download;
        }
    }
}
