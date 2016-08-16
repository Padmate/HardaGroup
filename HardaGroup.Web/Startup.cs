using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HardaGroup.Web.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
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
