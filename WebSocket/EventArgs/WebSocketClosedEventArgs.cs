using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.WebSocket
{
    public sealed class WebSocketClosedEventArgs : EventArgs
    {
        public readonly ushort Code;

        public readonly string Reason;

        public WebSocketClosedEventArgs(ushort code, string reason)
        {
            Code = code;
            Reason = reason;
        }
    }
}
