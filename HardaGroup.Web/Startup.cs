using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HardaGroup.Web.Startup))]
namespace HardaGroup.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
