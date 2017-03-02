using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SiiTaxi.Startup))]

namespace SiiTaxi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
