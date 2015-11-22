using DemoApp;

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
