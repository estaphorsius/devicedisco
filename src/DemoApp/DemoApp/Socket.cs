using System;
using System.Net;
using System.Net.Sockets;

namespace DemoApp
{
    public class DiscoSocket : ISocket
    {
        private readonly Socket _socket;
        public DiscoSocket(Socket socket)
        {
            _socket = socket;
        }

        public void BeginReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint, AsyncCallback asyncCallback,
            object state)
        {
            _socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref remoteEndPoint, asyncCallback, state);
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

        public int SendTo(byte[] buffer, SocketFlags flags, EndPoint remoteEndPoint)
        {
            return _socket.SendTo(buffer, SocketFlags.None, remoteEndPoint);
        }

        public void Close()
        {
            _socket.Close();
        }
    }
}