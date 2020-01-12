using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UWPUtilities.UseCase
{
    public interface IUseCaseRequest
    {
        RequestType Type { get; }

        CancellationTokenSource CTS { get; }
    }

    public class UseCaseRequest : IUseCaseRequest
    {
        public RequestType Type { get; private set; }

        public CancellationTokenSource CTS { get; private set; }

        public UseCaseRequest(RequestType type, CancellationTokenSource cts = default)
        {
            Type = type;
            CTS = cts;
        }
    }

    public class AuthenticatedUseCaseRequest : UseCaseRequest
    {
        public string UserName { get; set; }

        public AuthenticatedUseCaseRequest(RequestType type, string userName, CancellationTokenSource cts = default) : base(type, cts)
        {
            UserName = userName;
        }
    }
}
