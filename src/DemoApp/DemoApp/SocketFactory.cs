using System.Net;
using System.Net.Sockets;

namespace DemoApp
{
    public class SocketFactory : ISocketFactory
    {
        public Socket CreateListeningSocket()
        {
            Socket result = null;
            EndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Constants.MulticastPort);
            result = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            result.Bind(localEndPoint);
            result.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(IPAddress.Parse(Constants.MulticastAddress)));
            result.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

            return result;
        }

        public Socket CreateClientSocket()
        {
            var result = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            result.Bind(new IPEndPoint(IPAddress.Any, Constants.MulticastPort));
            result.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(IPAddress.Parse(Constants.MulticastAddress), IPAddress.Any));
            return result;
        }
    }
}