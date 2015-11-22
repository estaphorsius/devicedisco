using System;
using System.Text.RegularExpressions;

namespace DeviceDiscovery
{
    public class DeviceInformation
    {
        public string HostAddress { get; set; }
        public string UniqueId { get; set; }
        public string Location { get; set; }
        public string PlatformName { get; set; }
        public string PlatformVersion { get; set; }
        public DateTime LastSeen { get; set; }

        public static DeviceInformation CreateFromMessage(Message message)
        {
            var device = new DeviceInformation();

            if (message.Headers.ContainsKey("USN"))
            {
                device.UniqueId = message.Headers["USN"];
            }

            if (message.Headers.ContainsKey("LOCATION"))
            {
                device.Location = message.Headers["LOCATION"];
                Regex regex = new Regex(@"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");
                if (regex.IsMatch(device.Location))
                {
                    var m = regex.Match(device.Location);
                    device.HostAddress = m.Groups[0].Value;
                }
            }

            if (message.Headers.ContainsKey("SERVER"))
            {
                device.PlatformName = message.Headers["SERVER"];
            }

            return device;
        }
    }
}