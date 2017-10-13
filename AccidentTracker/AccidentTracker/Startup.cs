using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccidentTracker.Startup))]
namespace AccidentTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
