using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.BackgroundTransfer.Download.Contract;

namespace UWPUtilities.BackgroundTransfer.Download
{
    public class BackgroundDownloadResponse : IBackgroundDownloadResponse
    {
        public Uri RequestUri => throw new NotImplementedException();
    }
}
