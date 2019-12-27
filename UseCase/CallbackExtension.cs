using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.UseCase
{
    public static class CallbackExtension
    {
        public static void OnSuccess<R>(this ICallback<R> callback, IUseCaseResponse<R> response,
            Predicate<IUseCaseResponse<R>> predicate)
        {
            if (predicate(response))
            {
                callback?.OnSuccess(response);
            }
        }

        public static void OnError<R>(this ICallback<R> callback, ErrorType type, Exception exception)
        {
            callback?.OnError(new UseCaseError(type, exception));
        }

        public static void OnCanceled<R>(this ICallback<R> callback, ResponseType type = ResponseType.Unknown)
        {
            callback?.OnCanceled(new UseCaseResponse<R>(type, ResponseStatus.Canceled, data: default));
        }

        public static void OnFailed<R>(this ICallback<R> callback, ResponseType type = ResponseType.Unknown, ResponseStatus status = ResponseStatus.Failed)
        {
            callback?.OnFailed(new UseCaseResponse<R>(type, status, data: default));
        }
    }
}
