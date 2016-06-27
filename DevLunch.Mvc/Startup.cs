using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevLunch.Mvc.Startup))]
namespace DevLunch.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
