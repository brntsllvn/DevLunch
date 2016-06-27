using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DevLunch.Startup))]
namespace DevLunch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}