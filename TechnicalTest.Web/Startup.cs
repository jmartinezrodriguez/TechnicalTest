using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TechnicalTest.Web.Startup))]
namespace TechnicalTest.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
