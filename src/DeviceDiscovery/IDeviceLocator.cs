using System;

namespace DeviceDiscovery
{
    public interface IDeviceLocator
    {
        void FindDevices(TimeSpan timeout);
        event EventHandler<DeviceInformation> DeviceDiscovered;
    }
}