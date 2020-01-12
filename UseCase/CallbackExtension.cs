using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.UseCase
{
    public static class CallbackExtension
    {
        public static void OnSuccessOrFailed<R>(this ICallback<R> callback, UseCaseResponse<R> response,
            Predicate<UseCaseResponse<R>> predicate)
        {
            if (predicate(response))
            {
                callback?.OnSuccess(response);
            }
            else
            {
                callback?.OnFailed(response.Type, ResponseStatus.Failed);
            }
        }

        public static void OnSuccessOrFailed<R>(this ICallback<R> callback, ResponseType responseType, R response, Predicate<R> predicate)
        {
            if (predicate(response))
            {
                callback?.OnSuccess(responseType, ResponseStatus.Success, response);
            }
            else
            {
                callback?.OnFailed(responseType, ResponseStatus.Failed);
            }
        }

        public static void OnSuccess<R>(this ICallback<R> callback, ResponseType type, ResponseStatus status, R response)
        {
            callback?.OnSuccess(new UseCaseResponse<R>(type, status, response));
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
