using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPUtilities.Error;
using UWPUtilities.Extension;

namespace UWPUtilities.UseCase
{
    public abstract class UseCaseBase<TRequest, TResponseType> where TRequest : IUseCaseRequest
    {
        public TRequest Request { get; private set; }

        protected ICallback<TResponseType> PresenterCallback { get; private set; }

        private CancellationToken CancellationToken { get { return Request?.CTS?.Token ?? default; } }

        public UseCaseBase(TRequest request, ICallback<TResponseType> presenterCallback = null)
        {
            Request = request;
            PresenterCallback = presenterCallback;
        }

        public void Execute()
        {
            List<Exception> exceptions = default;
            try
            {
                GetFromCache();
            }
            catch (Exception cacheException)
            {
                exceptions.InitializeIfNull();
                exceptions.Add(cacheException);
            }
            try
            {
                Task.Run(() =>
                {
                    Action();
                }, CancellationToken).ConfigureAwait(false);
            }
            catch (NoInternetAccessException nie)
            {
                PresenterCallback?.OnError(ErrorType.Network, nie);
            }
            catch (OperationCanceledException)
            {
                PresenterCallback?.OnCanceled(ResponseType.Unknown);
            }
            catch (Exception exception)
            {
                exceptions.InitializeIfNull();
                exceptions.Add(exception);
            }
            finally
            {
                if (exceptions.IsNonEmpty())
                {
                    PresenterCallback?.OnError(ErrorType.Unknown, new AggregateException(exceptions));
                }
            }
        }

        public virtual bool GetFromCache()
        {
            return false;
        }

        public abstract void Action();
    }
}
