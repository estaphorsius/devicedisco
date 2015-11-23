using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DeviceDiscovery
{
    public class DiscoveryListener : IDiscoveryListener
    {
        private readonly ISocketFactory _socketFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IMessageParser _messageParser;
        private readonly IDeviceInfoCollector _deviceInfoCollector;
        private readonly ILog _log = LogManager.GetLogger(typeof(DiscoveryListener));

        private ISocket _listeningSocket;
        private bool _started;
        private bool _stopped;

        private class ListenerState
        {
            public ListenerState()
            {
                Buffer = new byte[1024];
            }

            public byte[] Buffer { get; private set; }
        }

        public DiscoveryListener(ISocketFactory socketFactory, IResponseFactory responseFactory, IMessageParser messageParser, IDeviceInfoCollector deviceInfoCollector)
        {
            _socketFactory = socketFactory;
            _responseFactory = responseFactory;
            _messageParser = messageParser;
            _deviceInfoCollector = deviceInfoCollector;
            _stopped = true;
            _started = false;
        }

        public void Listen()
        {
            _listeningSocket = _socketFactory.CreateListeningSocket();

            EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, Constants.MulticastPort);
            var listenerState = new ListenerState();

            _listeningSocket.BeginReceiveFrom(listenerState.Buffer,
                ref remoteEndpoint, OnReceive, listenerState);

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
        public event EventHandler<Message> SearchMessageReceived;

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                if (_listeningSocket != null)
                {
                    EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, Constants.MulticastPort);
                    var bytesReceived = _listeningSocket.EndReceiveFrom(ar, ref remoteEndpoint);
                    var listenerState = (ListenerState)ar.AsyncState;
                    var requestString = Encoding.ASCII.GetString(listenerState.Buffer, 0, bytesReceived);
                    var message = _messageParser.Parse(requestString);

                    if (message.MessageLine.StartsWith(Constants.NotifyMessage))
                    {
                        Task.Run(() =>
                        {
                            var dev = _deviceInfoCollector.Collect(message);
                            this.DeviceDiscovered?.Invoke(this, dev);
                        });

                    }
                    else if (message.MessageLine.StartsWith(Constants.SearchMessage))
                    {
                        this.SearchMessageReceived?.Invoke(this, message);

                        var responseString = _responseFactory.CreateResponse(requestString);

                        if (responseString != null)
                        {
                            var responseBuffer = Encoding.ASCII.GetBytes(responseString);
                            _listeningSocket.SendTo(responseBuffer, remoteEndpoint);
                        }
                    }

                    // enter new listening loop
                    listenerState = new ListenerState();
                    _listeningSocket.BeginReceiveFrom(listenerState.Buffer,
                        ref remoteEndpoint, OnReceive, listenerState);
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