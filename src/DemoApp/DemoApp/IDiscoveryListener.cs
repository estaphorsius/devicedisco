using System;

namespace DemoApp
{
    interface IDiscoveryListener
    {
        void Listen();
        void Stop();
        event EventHandler<DeviceInformation> DeviceDiscovered;
    }
}