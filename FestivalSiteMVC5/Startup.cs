using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FestivalSiteMVC5.Startup))]
namespace FestivalSiteMVC5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
