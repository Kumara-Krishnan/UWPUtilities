using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.WebSocket.Contract;
using Windows.Networking.Sockets;

namespace UWPUtilities.WebSocket
{
    public sealed class MessageWebSocketAdapter : WebSocketAdapterBase<MessageWebSocket>, IMessageWebSocketAdapter
    {

    }
}
