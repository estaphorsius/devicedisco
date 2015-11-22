using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace DeviceDiscovery
{
    public class DeviceInformation
    {
        public string HostAddress { get; set; }
        public string UniqueId { get; set; }
        public string Location { get; set; }
        public string PlatformName { get; set; }
        public string PlatformVersion { get; set; }
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }
        public string PresentationUrl { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerUrl { get; set; }
        public string FriendlyName { get; set; }

        public DateTime LastSeen { get; set; }

    }
}