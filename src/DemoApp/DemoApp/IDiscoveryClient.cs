using System;

namespace DemoApp
{
    interface IDiscoveryClient
    {
        void Search();
        event EventHandler SearchResponseReceived;
    }
}