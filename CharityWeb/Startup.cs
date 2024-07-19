using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CharityWeb.Startup))]
namespace CharityWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
