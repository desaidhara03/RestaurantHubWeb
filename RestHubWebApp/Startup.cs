using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestHubWebApp.Startup))]
namespace RestHubWebApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
