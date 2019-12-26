using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.BackgroundTransfer.Download.Contract;
using UWPUtilities.BackgroundTransfer.Upload.Contract;
using UWPUtilities.Collections;
using UWPUtilities.Util;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace UWPUtilities.BackgroundTransfer
{
    public sealed class BackgroundTransferManager
    {
        #region Singleton

        public static readonly BackgroundTransferManager Instance = new BackgroundTransferManager();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static BackgroundTransferManager() { }

        private BackgroundTransferManager() { }

        #endregion

        #region Download

        public int ParallelDownloadLimit { get; set; } = 10;

        private readonly ConcurrentBag<IBackgroundDownloadHandler<IBackgroundDownloadRequest, IBackgroundDownloadResponse>> CurrentDownloads =
            new ConcurrentBag<IBackgroundDownloadHandler<IBackgroundDownloadRequest, IBackgroundDownloadResponse>>();

        private readonly ConcurrentQueue<IBackgroundDownloadHandler<IBackgroundDownloadRequest, IBackgroundDownloadResponse>> QueuedDownloads =
            new ConcurrentQueue<IBackgroundDownloadHandler<IBackgroundDownloadRequest, IBackgroundDownloadResponse>>();

        public Task DownloadAsync(IBackgroundDownloadHandler<IBackgroundDownloadRequest, IBackgroundDownloadResponse> handler)
        {
            try
            {
                if (CanExecuteDownload(handler))
                {
                    CurrentDownloads.Add(handler);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                var queuedDownload = QueuedDownloads.Take(1).FirstOrDefault();
                if (queuedDownload != null)
                {
                    DownloadAsync(queuedDownload);
                }
            }

            return Task.CompletedTask;
        }

        public Task PauseDownloadAsync(string transferId)
        {
            return Task.CompletedTask;
        }

        public Task ResumeDownloadAsync(string transferId)
        {
            return Task.CompletedTask;
        }

        public Task CancelDownloadAsync(string transferId)
        {
            return Task.CompletedTask;
        }

        private bool CanExecuteDownload(IBackgroundDownloadHandler<IBackgroundDownloadRequest, IBackgroundDownloadResponse> handler)
        {
            if (CurrentDownloads.Count >= ParallelDownloadLimit)
            {
                QueuedDownloads.Enqueue(handler);
                return false;
            }
            return true;
        }

        #endregion

        #region Upload

        public Task UploadAsync(IBackgroundUploadHandler<IBackgroundUploadRequest, IBackgroundUploadResponse> handler)
        {
            return Task.CompletedTask;
        }

        public Task CancelUploadAsync(string transferId)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
