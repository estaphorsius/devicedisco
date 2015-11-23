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
            var result = new DiscoSocket();
            return result;
        }
    }
}