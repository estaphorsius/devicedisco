namespace DeviceDiscovery
{
    public interface IDeviceInfoCollector
    {
        DeviceInformation Collect(Message message);
    }
}