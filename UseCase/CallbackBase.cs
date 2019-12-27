using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.UseCase
{
    public interface ICallback<R>
    {
        void OnSuccess(IUseCaseResponse<R> response);

        void OnError(UseCaseError error);

        void OnFailed(IUseCaseResponse<R> response);

        void OnCanceled(IUseCaseResponse<R> response);
    }

    public interface ICallbackWithProgress<R> : ICallback<R>
    {
        void OnProgress(IUseCaseResponse<R> response);
    }

    public abstract class CallbackBase<R> : ICallbackWithProgress<R>
    {
        public abstract void OnSuccess(IUseCaseResponse<R> response);

        public abstract void OnError(UseCaseError error);

        public void OnCanceled(IUseCaseResponse<R> response) { }

        public void OnFailed(IUseCaseResponse<R> response) { }

        public void OnProgress(IUseCaseResponse<R> response) { }
    }
}
