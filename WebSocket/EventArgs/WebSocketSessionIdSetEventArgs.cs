using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.WebSocket
{
    public sealed class WebSocketSessionIdSetEventArgs : EventArgs
    {
        public readonly string SessionId;

        public WebSocketSessionIdSetEventArgs(string sessionId)
        {
            SessionId = sessionId;
        }
    }
}
