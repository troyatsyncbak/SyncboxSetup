using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Syncbak.SyncboxSetup.Startup))]
namespace Syncbak.SyncboxSetup
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
