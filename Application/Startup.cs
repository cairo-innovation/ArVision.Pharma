using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pharma.Startup))]
namespace Pharma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
