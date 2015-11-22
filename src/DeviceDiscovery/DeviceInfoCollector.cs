using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace DeviceDiscovery
{
    public class DeviceInfoCollector : IDeviceInfoCollector
    {
        public DeviceInformation Collect(Message message)
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

                HandleLocation(device);
            }

            if (message.Headers.ContainsKey("SERVER"))
            {
                device.PlatformName = message.Headers["SERVER"];
            }

            return device;
        }

        private static void HandleLocation(DeviceInformation device)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    string xmlData = wc.DownloadString(device.Location);

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlData);
                    if (xmlDoc.DocumentElement != null)
                    {
                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                        nsmgr.AddNamespace("dev", xmlDoc.DocumentElement.NamespaceURI);
                        var name = xmlDoc.SelectSingleNode("/dev:root/dev:device/dev:friendlyName", nsmgr);
                        var presentationUrl = xmlDoc.SelectSingleNode("/dev:root/dev:device/dev:presentationURL", nsmgr);
                        var modelName = xmlDoc.SelectSingleNode("/dev:root/dev:device/dev:modelName", nsmgr);
                        var modelDesc = xmlDoc.SelectSingleNode("/dev:root/dev:device/dev:modelDescription", nsmgr);

                        device.FriendlyName = name?.InnerText;
                        device.PresentationUrl = presentationUrl?.InnerText;
                        device.ModelName = modelName?.InnerText;
                        device.ModelDescription = modelDesc?.InnerText;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}