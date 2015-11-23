using System.Collections;
using System.Collections.Generic;
using System.Web.Http;

namespace DeviceDiscovery.TestApp.Api
{
    [RoutePrefix("api/device")]
    public class DeviceListController : ApiController
    {
        [HttpGet, Route("list")]
        public IEnumerable<DeviceInformation> List()
        {
            return DiscoveredDevices.ToList();
        }
    }
}