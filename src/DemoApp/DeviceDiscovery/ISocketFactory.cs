namespace DeviceDiscovery
{
    public interface ISocketFactory
    {
        ISocket CreateListeningSocket();
        ISocket CreateClientSocket();
    }
}