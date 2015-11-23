using System.Configuration;
using System.Net.Http.Formatting;
using System.Web.Http;
using DeviceDiscovery.TestApp;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(OwinStartup))]

namespace DeviceDiscovery.TestApp
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileSystem = new PhysicalFileSystem(ConfigurationManager.AppSettings["wwwroot"])
            });
        }
    }
}
