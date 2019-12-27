using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.WebSocket.Contract;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace UWPUtilities.WebSocket
{
    public sealed class StreamWebSocketAdapter : WebSocketAdapterBase<StreamWebSocket>, IStreamWebSocketAdapter
    {
        public void test()
        {
            using (var dataReader = new DataReader(WebSocket.InputStream))
            {

            }
        }
    }
}
