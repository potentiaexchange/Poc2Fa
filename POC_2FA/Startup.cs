using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProvaDeConceito2FA.Startup))]
namespace ProvaDeConceito2FA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
