using System;

namespace DemoApp
{
    interface IDiscoveryService
    {
        void Listen();
        event EventHandler NotificationReceived;
        event EventHandler DiscoveryRequestReceived;
    }
}