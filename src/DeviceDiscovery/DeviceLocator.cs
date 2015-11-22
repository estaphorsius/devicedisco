using System;
using System.Net;
using System.Text;
using System.Threading;

namespace DeviceDiscovery
{
    public class DeviceLocator : IDeviceLocator
    {
        private readonly ISocketFactory _socketFactory;
        private readonly IRequestFactory _requestFactory;
        private readonly IMessageParser _messageParser;
        private readonly IDeviceInfoCollector _deviceInfoCollector;
        private ISocket _socket;

        public DeviceLocator(ISocketFactory socketFactory, IRequestFactory requestFactory, IMessageParser messageParser, IDeviceInfoCollector deviceInfoCollector)
        {
            _socketFactory = socketFactory;
            _requestFactory = requestFactory;
            _messageParser = messageParser;
            _deviceInfoCollector = deviceInfoCollector;
        }


        public event EventHandler<DeviceInformation> DeviceDiscovered;

        public void FindDevices(TimeSpan timeout)
        {
            _socket = _socketFactory.CreateClientSocket();
            var request = _requestFactory.CreateMessage();
            var requestBytes = Encoding.ASCII.GetBytes(request);

            var responseThread = new Thread(GetSearchResponse);
            _socket.SendTo(requestBytes, new IPEndPoint(IPAddress.Parse(Constants.MulticastAddress), Constants.MulticastPort));
            responseThread.Start();
            Thread.Sleep(Convert.ToInt32(timeout.TotalMilliseconds));
            _socket.Close();
        }

        private void GetSearchResponse()
        {
            try
            {
                while (true)
                {
                    var response = new byte[8000];
                    EndPoint ep = new IPEndPoint(IPAddress.Any, Constants.MulticastPort);
                    var receivedByteCount = _socket.ReceiveFrom(response, ref ep);

                    var str = Encoding.UTF8.GetString(response, 0, receivedByteCount);
                    var msg = _messageParser.Parse(str);
                    if (msg.MessageLine == "HTTP/1.1 200 OK")
                    {
                        var dev = _deviceInfoCollector.Collect(msg);
                        this.DeviceDiscovered?.Invoke(this, dev);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}