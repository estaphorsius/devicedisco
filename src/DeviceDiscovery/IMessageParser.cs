namespace DeviceDiscovery
{
    public interface IMessageParser
    {
        Message Parse(string rawMessage);
    }
}