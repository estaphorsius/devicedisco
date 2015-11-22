using System;

namespace DeviceDiscovery
{
    public interface IDiscoveryListener
    {
        void Listen();
        void Stop();
        event EventHandler<DeviceInformation> DeviceDiscovered;
    }
}