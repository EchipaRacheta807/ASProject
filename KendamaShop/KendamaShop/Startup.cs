using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KendamaShop.Startup))]
namespace KendamaShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
