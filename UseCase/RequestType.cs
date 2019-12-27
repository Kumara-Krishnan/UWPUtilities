using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.UseCase
{
    public enum RequestType
    {
        LocalStorage,
        Network,
        LocalAndNetwork,
        Sync
    }

    public static class RequestTypeExtension
    {
        public static bool HasLocalStorage(this RequestType requestType)
        {
            return requestType == RequestType.LocalAndNetwork || requestType == RequestType.LocalStorage
                || requestType == RequestType.Sync;
        }

        public static bool HasNetwork(this RequestType requestType)
        {
            return requestType == RequestType.LocalAndNetwork || requestType == RequestType.Network
                || requestType == RequestType.Network;
        }
    }
}
