using System;
using System.Threading;
using DeviceDiscovery;
using log4net;
using Microsoft.Owin.Hosting;

namespace DeviceDiscovery.TestApp
{
    class Program
    {
        private static Timer _searchTimer;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
        private static IDiscoveryListener _discoveryListener;
        private static IDeviceLocator _deviceLocator;


        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            //WebApp.Start<OwinStartup>("http://+:4500/");
            var messageParser = new MessageParser();
            var deviceInformation = new DeviceInformation
            {
                Location = "http://192.168.0.251/device.xml",
                HostAddress = "192.168.0.30",
                UniqueId = "DEAD-BEEF",
                PlatformName = "WINDOWS",
                PlatformVersion = "10.0"
            };


            var responseFactory = new ResponseFactory(deviceInformation, messageParser);
            _deviceLocator = new DeviceLocator(new SocketFactory(), new SearchRequestFactory(), new MessageParser(), new DeviceInfoCollector());
            _deviceLocator.DeviceDiscovered += OnDeviceDiscovered;
            _discoveryListener = new DiscoveryListener(new SocketFactory(), responseFactory, messageParser, new DeviceInfoCollector());
            _discoveryListener.DeviceDiscovered += OnDeviceDiscovered;
            // _discoveryListener.Listen();

            _searchTimer = new Timer(
                SearchTimerFunc,
                null,
                TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(30));

            Console.Read();
        }

        private static void OnDeviceDiscovered(object sender, DeviceInformation e)
        {
            DiscoveredDevices.AddOrUpdate(e);
        }

        private static void SearchTimerFunc(object state)
        {
            try
            {
                _discoveryListener.Stop();
                _deviceLocator.FindDevices(TimeSpan.FromSeconds(15));
                _discoveryListener.Listen();
            }
            catch (Exception e)
            {
                Log.Error(e);
                _discoveryListener.Listen();
            }
        }
    }
}
