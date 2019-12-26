using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.BackgroundTransfer;
using UWPUtilities.BackgroundTransfer.Download;

namespace UWPUtilities_Test.BackgroundTransfer_Tests
{
    [TestClass]
    public class BackgroundTransferManagerTest
    {
        [TestMethod]
        public Task DownloadAsync()
        {
            return BackgroundTransferManager.Instance.DownloadAsync(new BackgroundDownloadHandler());
        }
    }
}
