using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(侍.Startup))]
namespace 侍
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
