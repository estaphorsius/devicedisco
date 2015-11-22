using System.Configuration;
using System.Web.Http;
using DeviceDiscovery.TestApp;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

[assembly: OwinStartup(typeof(OwinStartup))]

namespace DeviceDiscovery.TestApp
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();
            app.UseWebApi(httpConfig);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileSystem = new PhysicalFileSystem(ConfigurationManager.AppSettings["wwwroot"])
            });
        }
    }
}
