using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace UWPUtilities.WebSocket.Contract
{
    public delegate void WebSocketConnectedEventHandler(IWebSocketAdapter sender, WebSocketConnectedEventArgs e);

    public delegate void WebSocketSessionIdSetEventHandler(IWebSocketAdapter sender, WebSocketSessionIdSetEventArgs e);

    public delegate void WebSocketMessageReceivedEventHandler(IWebSocketAdapter sender, string message);

    public delegate void WebSocketErrorEventHandler(IWebSocketAdapter sender, WebSocketErrorEventArgs e);

    public delegate void WebSocketClosedEventHandler(IWebSocketAdapter sender, WebSocketClosedEventArgs e);

    public interface IWebSocketAdapter
    {
        Task<bool> ConnectAsync(string uri, bool forceConnect = false);

        Task<bool> ConnectAsync(string uri, Dictionary<string, string> requestHeaders, bool forceConnect = false);

        void Close();

        void Close(ushort code, string reason);
    }
}
