using System;
using System.Net;
using System.Net.Sockets;

namespace DeviceDiscovery
{
    public class DiscoSocket : ISocket
    {
        private readonly Socket _socket;
        public DiscoSocket()
        {
            Socket socket = null;
            EndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Constants.MulticastPort);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(localEndPoint);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(IPAddress.Parse(Constants.MulticastAddress)));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

            _socket = socket;
        }

        public IAsyncResult BeginReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint, AsyncCallback asyncCallback, object state)
        {
            return _socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref remoteEndPoint, asyncCallback, state);
        }

        public int EndReceiveFrom(IAsyncResult ar, ref EndPoint remoteEndPoint)
        {
            return _socket.EndReceiveFrom(ar, ref remoteEndPoint);
        }

        public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint)
        {
            return _socket.ReceiveFrom(buffer, ref remoteEndPoint);
        }

        public int SendTo(byte[] buffer, EndPoint remoteEndPoint)
        {
            return _socket.SendTo(buffer, remoteEndPoint);
        }

        public void Close()
        {
            _socket.Close();
        }
    }
}