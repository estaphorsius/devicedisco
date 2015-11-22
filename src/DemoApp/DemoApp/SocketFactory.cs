using System.Net;
using System.Net.Sockets;

namespace DemoApp
{
    public class SocketFactory : ISocketFactory
    {
        public ISocket CreateListeningSocket()
        {
            Socket socket = null;
            EndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Constants.MulticastPort);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(localEndPoint);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(IPAddress.Parse(Constants.MulticastAddress)));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

            var result = new DiscoSocket(socket);
            return result;
        }

        public ISocket CreateClientSocket()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Any, Constants.MulticastPort));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(IPAddress.Parse(Constants.MulticastAddress), IPAddress.Any));

            var result = new DiscoSocket(socket);
            return result;
        }
    }
}