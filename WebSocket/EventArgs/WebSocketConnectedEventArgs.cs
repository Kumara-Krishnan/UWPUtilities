using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.WebSocket
{
    public sealed class WebSocketConnectedEventArgs : EventArgs
    {
        public readonly bool IsSessionIdSet;

        public readonly string SessionId;

        public readonly bool IsAutoReconnect;

        public readonly int AttemptCount;

        public WebSocketConnectedEventArgs(bool isSessionIdSet)
        {
            IsSessionIdSet = isSessionIdSet;
        }

        public WebSocketConnectedEventArgs(bool isSessionIdSet, string sessionId) : this(isSessionIdSet)
        {
            SessionId = sessionId;
        }

        public WebSocketConnectedEventArgs(bool isSessionIdSet, string sessionId, bool isAutoReconnect, int attemptCount)
            : this(isSessionIdSet, sessionId)
        {
            IsAutoReconnect = isAutoReconnect;
            AttemptCount = attemptCount;
        }
    }
}
