using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DemoApp
{
    public class DeviceLocator : IDeviceLocator
    {
        private readonly ISocketFactory _socketFactory;
        private readonly IRequestFactory _requestFactory;
        private readonly IMessageParser _messageParser;
        private ISocket _socket;

        public DeviceLocator(ISocketFactory socketFactory, IRequestFactory requestFactory, IMessageParser messageParser)
        {
            _socketFactory = socketFactory;
            _requestFactory = requestFactory;
            _messageParser = messageParser;
        }


        public void FindDevices(TimeSpan timeout)
        {
            _socket = _socketFactory.CreateClientSocket();
            string request = _requestFactory.CreateMessage();
            byte[] requestBytes = Encoding.ASCII.GetBytes(request);

            var responseThread = new Thread(GetSearchResponse);
            _socket.SendTo(requestBytes, SocketFlags.None,
                new IPEndPoint(IPAddress.Parse(Constants.MulticastAddress), Constants.MulticastPort));
            responseThread.Start();
            Thread.Sleep(Convert.ToInt32(timeout.TotalMilliseconds));
            _socket.Close();
        }

        public event EventHandler<DeviceInformation> DeviceDiscovered;



        private void GetSearchResponse()
        {
            try
            {
                while (true)
                {
                    var response = new byte[8000];
                    EndPoint ep = new IPEndPoint(IPAddress.Any, Constants.MulticastPort);
                    int receivedByteCount = _socket.ReceiveFrom(response, ref ep);

                    var str = Encoding.UTF8.GetString(response, 0, receivedByteCount);
                    Message msg = _messageParser.Parse(str);
                    if (msg.MessageLine == "HTTP/1.1 200 OK")
                    {
                        DeviceInformation dev = DeviceInformation.CreateFromMessage(msg);
                        this.DeviceDiscovered?.Invoke(this, dev);
                    }
                }
            }
            catch
            {
                //TODO handle exception for when connection closes
            }
        }
    }
}