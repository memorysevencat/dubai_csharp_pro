using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DuBai_Pro.Startup))]
namespace DuBai_Pro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
