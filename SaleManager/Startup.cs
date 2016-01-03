using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SaleManager.Startup))]
namespace SaleManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
