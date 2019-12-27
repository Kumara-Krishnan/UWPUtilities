using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.BackgroundTransfer.Upload.Contract;

namespace UWPUtilities.BackgroundTransfer.Upload
{
    public class BackgroundUploadResponse : IBackgroundUploadResponse
    {
        public Uri RequestUri => throw new NotImplementedException();
    }
}
