using System;
using System.Net;
using System.Net.Sockets;

namespace DemoApp
{
    public interface ISocket
    {
        void BeginReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint, AsyncCallback asynCallback, object state);
        int EndReceiveFrom(IAsyncResult ar, ref EndPoint remoteEndPoint);
        int ReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint);
        int SendTo(byte[] buffer, EndPoint remoteEndPoint);
        int SendTo(byte[] buffer, SocketFlags flags, EndPoint ep);
        void Close();
    }
}