using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Extension;
using UWPUtilities.Extension;
using UWPUtilities.WebSocket.Contract;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.System.Threading;

namespace UWPUtilities.WebSocket
{
    /// <summary>
    /// https://tools.ietf.org/html/rfc6455
    /// </summary>
    public abstract class WebSocketAdapterBase<T> : IWebSocketAdapter where T : IWebSocket
    {
        public event WebSocketConnectedEventHandler Connected;

        public event WebSocketSessionIdSetEventHandler SessionIdSet;

        public event WebSocketMessageReceivedEventHandler MessageReceived;

        public event WebSocketErrorEventHandler Error;

        public event WebSocketClosedEventHandler Closed;

        public Uri Uri { get; protected set; }

        public bool IsPingPongEnabled { get; set; } = true;

        public string PingMessage { get; set; } = "--/--";

        public string PongMessage { get; set; } = "--/--";

        public TimeSpan PingInterval { get; set; } = TimeSpan.FromSeconds(30);

        private ThreadPoolTimer PingTimer { get; set; }

        private bool IsPingInProgress;

        public bool IsAutoReconnectEnabled { get; set; } = true;

        public int ReconnectAttempt { get; protected set; } = 0;

        public int? MaxReconnectAttempts { get; protected set; }

        public double[] ReconnectIntervals = new double[] { 0, 15, 30, 60, 120, 240 };

        protected readonly T WebSocket;

        private bool IsConnected { get; set; }

        public WebSocketAdapterBase()
        {
            WebSocket = Activator.CreateInstance<T>();
            WebSocket.Closed += OnWebSocketClosed;
        }

        private void OnWebSocketClosed(IWebSocket sender, Windows.Networking.Sockets.WebSocketClosedEventArgs args)
        {
            Closed?.Invoke(this, new WebSocketClosedEventArgs(args.Code, args.Reason));
        }

        public virtual async Task<bool> ConnectAsync(string uri, bool forceConnect = false)
        {
            return await ConnectAsync(uri, requestHeaders: default, forceConnect);
        }

        public virtual async Task<bool> ConnectAsync(string uri, Dictionary<string, string> requestHeaders, bool forceConnect = false)
        {
            SetRequestHeaders(requestHeaders);
            if (!IsConnected || forceConnect)
            {
                Close();
                ValidateUri(uri, out Uri requestUri);
                Uri = requestUri;
                await WebSocket.ConnectAsync(Uri);
                IsConnected = true;
                InitializePingTimer();
            }
            return IsConnected;
        }

        public Task<bool> SendMessageAsync(string message)
        {
            return SendMessasgeInternalAsync(async () =>
            {
                using (var dataWriter = new DataWriter(WebSocket.OutputStream))
                {
                    dataWriter.WriteString(message);
                    await dataWriter.StoreAsync();
                    dataWriter.DetachStream();
                }
            });
        }

        private async Task<bool> SendMessasgeInternalAsync(Func<Task> sendMessageFunc)
        {
            bool isSuccess;
            try
            {
                if (!IsConnected) { throw new InvalidOperationException("Websocket disconnected"); }
                await sendMessageFunc();
                isSuccess = true;
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public void Close()
        {
            if (IsConnected)
            {
                Close(1000, string.Empty);
            }
        }

        public void Close(ushort code, string reason)
        {
            try
            {
                WebSocket.Close(code, reason);
            }
            catch
            {

            }
        }

        private void InitializePingTimer()
        {
            if (IsPingPongEnabled)
            {
                PingTimer = ThreadPoolTimer.CreatePeriodicTimer(OnPingTimerElapsed, PingInterval);
            }
        }

        private async void OnPingTimerElapsed(ThreadPoolTimer timer)
        {
            if (!IsPingInProgress)
            {
                IsPingInProgress = true;
                await SendMessageAsync(PingMessage);
                IsPingInProgress = false;
            }
        }

        private void SetRequestHeaders(Dictionary<string, string> requestHeaders)
        {
            if (requestHeaders.IsNullOrEmpty()) { return; }
            foreach (var requestHeader in requestHeaders)
            {
                WebSocket.SetRequestHeader(requestHeader.Key, requestHeader.Value);
            }
        }

        private bool ValidateUri(string url, out Uri uri)
        {
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                return true;
            }
            throw new ArgumentException("Invalid url");
        }
    }
}
