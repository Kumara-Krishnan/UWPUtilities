using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Web.Http;

namespace UWPUtilities.BackgroundTransfer.Contract
{
    public interface IBackgroundTransferRequest : IBackgroundTransferBase
    {
        HttpMethod Method { get; }

        BackgroundTransferPriority Priority { get; }

        BackgroundTransferGroup TransferGroup { get; }

        BackgroundTransferCompletionGroup CompletionGroup { get; }
    }
}
