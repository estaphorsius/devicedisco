using System.Net.Sockets;

namespace DemoApp
{
    public interface ISocketFactory
    {
        Socket CreateListeningSocket();
        Socket CreateClientSocket();
    }
}