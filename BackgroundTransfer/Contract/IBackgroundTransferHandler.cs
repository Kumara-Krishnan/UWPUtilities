using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.BackgroundTransfer.Contract
{
    public interface IBackgroundTransferHandler<out TRequest, out TResponse> where TRequest : IBackgroundTransferRequest
        where TResponse : IBackgroundTransferResponse
    {
        TRequest Request { get; }

        TResponse Response { get; }
    }
}
