using Microsoft.Owin;
using Owin;
using SiiTaxi;

[assembly: OwinStartup(typeof(Startup))]

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