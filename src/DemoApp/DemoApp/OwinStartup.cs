using System.Web.Http;
using DemoApp;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(OwinStartup))]

namespace DemoApp
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            app.UseWebApi(httpConfig);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileSystem = new PhysicalFileSystem("..\\..")
            });
        }
    }
}
