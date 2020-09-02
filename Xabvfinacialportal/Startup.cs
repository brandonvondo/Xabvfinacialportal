using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Xabvfinacialportal.Startup))]
namespace Xabvfinacialportal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
