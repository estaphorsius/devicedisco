using System.Net.Sockets;

namespace DemoApp
{
    public interface ISocketFactory
    {
        ISocket CreateListeningSocket();
        ISocket CreateClientSocket();
    }
}