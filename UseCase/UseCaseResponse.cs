using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.UseCase
{
    public interface IUseCaseResponse<R>
    {
        ResponseType Type { get; }

        ResponseStatus Status { get; }

        R Data { get; }
    }

    public class UseCaseResponse<R> : IUseCaseResponse<R>
    {
        public ResponseType Type { get; private set; }

        public ResponseStatus Status { get; private set; }

        public R Data { get; private set; }

        public UseCaseResponse(ResponseType type, ResponseStatus status, R data)
        {
            Type = type;
            Status = status;
            Data = data;
        }
    }
}
