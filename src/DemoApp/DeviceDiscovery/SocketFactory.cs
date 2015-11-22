using System.Net;
using System.Net.Sockets;

namespace DeviceDiscovery
{
    public class SocketFactory : ISocketFactory
    {
        public ISocket CreateListeningSocket()
        {
            return new DiscoSocket();
        }

        public ISocket CreateClientSocket()
        {
            //var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //socket.Bind(new IPEndPoint(IPAddress.Any, Constants.MulticastPort));
            //socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
            //    new MulticastOption(IPAddress.Parse(Constants.MulticastAddress), IPAddress.Any));

            var result = new DiscoSocket();
            return result;
        }
    }
}