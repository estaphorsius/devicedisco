using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace DeviceDiscovery
{
    public static class DiscoveredDevices
    {
        private static readonly Hashtable Devices = Hashtable.Synchronized(new Hashtable());
        private static readonly ILog Log = LogManager.GetLogger(typeof(DiscoveredDevices));

        public static void AddOrUpdate(DeviceInformation deviceInformation)
        {
            deviceInformation.LastSeen = DateTime.Now;
            if (Devices.ContainsKey(deviceInformation.UniqueId))
            {
                Devices[deviceInformation.UniqueId] = deviceInformation;
                return;
            }

            Devices.Add(deviceInformation.UniqueId, deviceInformation);

            LogDevice(deviceInformation);
        }

        public static List<DeviceInformation> ToList()
        {
            var result = new List<DeviceInformation>(Devices.Count);
            result.AddRange(Devices.Values.Cast<DeviceInformation>());
            return result;
        }

        private static void LogDevice(DeviceInformation d)
        {
            Log.InfoFormat("Discovered a new device:");
            Log.InfoFormat("FriendlyName  : {0}", d.FriendlyName);
            Log.InfoFormat("ModelName     : {0}", d.ModelName);
            Log.InfoFormat("ModelDesc     : {0}", d.ModelDescription);
            Log.InfoFormat("HostAddress   : {0}", d.HostAddress);
            Log.InfoFormat("Location      : {0}", d.Location);
            Log.InfoFormat("UniqueId      : {0}", d.UniqueId);
            Log.InfoFormat("Platform      : {0}", d.PlatformName);
        }
    }
}
