using System;

namespace DemoApp
{
    public interface IDeviceLocator
    {
        void FindDevices(TimeSpan timeout);
        event EventHandler<DeviceInformation> DeviceDiscovered;
    }
}