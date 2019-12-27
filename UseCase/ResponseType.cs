using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.UseCase
{
    public enum ResponseType
    {
        Unknown,
        Cache,
        LocalStorage,
        Network,
        Sync
    }

    public enum ResponseStatus
    {
        Success,
        Error,
        Failed,
        Canceled
    }
}
