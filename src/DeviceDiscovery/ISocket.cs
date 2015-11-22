using System;
using System.Net;

namespace DeviceDiscovery
{
    public interface ISocket
    {
        IAsyncResult BeginReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint, AsyncCallback asynCallback, object state);
        int EndReceiveFrom(IAsyncResult ar, ref EndPoint remoteEndPoint);
        int ReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint);
        int SendTo(byte[] buffer, EndPoint remoteEndPoint);
        void Close();
    }
}