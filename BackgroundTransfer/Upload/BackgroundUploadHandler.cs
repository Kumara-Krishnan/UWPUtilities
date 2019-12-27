using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.BackgroundTransfer.Upload.Contract;

namespace UWPUtilities.BackgroundTransfer.Upload
{
    public class BackgroundUploadHandler : IBackgroundUploadHandler<BackgroundUploadRequest, BackgroundUploadResponse>
    {
        public BackgroundUploadRequest Request { get; set; }

        public BackgroundUploadResponse Response { get; set; }
    }
}
