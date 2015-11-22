using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    public class DiscoveryListener : IDiscoveryListener
    {
        private readonly ISocketFactory _socketFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IMessageParser _messageParser;
        private readonly ILog _log = LogManager.GetLogger(typeof(DiscoveryListener));

        private Socket _listeningSocket;
        private readonly object _locker = new object();
        private bool _started;
        private bool _stopped;

        private class ServerState
        {
            public ServerState()
            {
                Buffer = new byte[1024];
            }

            public byte[] Buffer { get; private set; }
        }

        public DiscoveryListener(ISocketFactory socketFactory, IResponseFactory responseFactory, IMessageParser messageParser)
        {
            _socketFactory = socketFactory;
            _responseFactory = responseFactory;
            _messageParser = messageParser;
            _stopped = true;
            _started = false;
        }

        public void Listen()
        {

            _listeningSocket = _socketFactory.CreateListeningSocket();

            EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, Constants.MulticastPort);
            var serverState = new ServerState();

            _listeningSocket.BeginReceiveFrom(serverState.Buffer, 0, serverState.Buffer.Length, SocketFlags.None,
                ref remoteEndpoint, OnReceive, serverState);

            _stopped = false;
            _started = true;
        }

        public void Stop()
        {

            if (!_started) return;

            _started = false;
            _stopped = true;
            _listeningSocket.Close();
            _listeningSocket = null;

        }

        public event EventHandler<DeviceInformation> DeviceDiscovered;

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                if (_listeningSocket != null)
                {
                    EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, Constants.MulticastPort);
                    var bytesReceived = _listeningSocket.EndReceiveFrom(ar, ref remoteEndpoint);
                    var serverState = (ServerState)ar.AsyncState;
                    var requestString = Encoding.ASCII.GetString(serverState.Buffer, 0, bytesReceived);
                    var message = _messageParser.Parse(requestString);
                    var dev = DeviceInformation.CreateFromMessage(message);
                    if (message.MessageLine.StartsWith("NOTIFY"))
                    {
                        Task.Run(() => this.DeviceDiscovered?.Invoke(this, dev));

                    }
                    else if (message.MessageLine.StartsWith("M-SEARCH"))
                    {
                        var responseString = _responseFactory.CreateResponse(requestString);

                        if (responseString != null)
                        {
                            var responseBuffer = Encoding.ASCII.GetBytes(responseString);
                            // send response
                            _listeningSocket.SendTo(responseBuffer, remoteEndpoint);
                        }
                    }

                    // enter new listening loop
                    serverState = new ServerState();
                    _listeningSocket.BeginReceiveFrom(serverState.Buffer, 0, serverState.Buffer.Length,
                        SocketFlags.None,
                        ref remoteEndpoint, OnReceive, serverState);
                }
            }
            catch (ObjectDisposedException)
            {

            }
            catch (Exception e)
            {
                _log.Error(e);

                if (_started && !_stopped)
                {
                    // stop and start listening again because there was an error 
                    Stop();
                    Listen();
                }
            }

        }
    }
}