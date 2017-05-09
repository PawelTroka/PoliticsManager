using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PoliticsManager.Startup))]
namespace PoliticsManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
