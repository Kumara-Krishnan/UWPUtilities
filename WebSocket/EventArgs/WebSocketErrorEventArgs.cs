using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Web;

namespace UWPUtilities.WebSocket
{
    public sealed class WebSocketErrorEventArgs : EventArgs
    {
        public WebErrorStatus ErrorStatus { get { return WebSocketError.GetStatus(Exception.HResult); } }

        public readonly Exception Exception;

        public WebSocketErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
