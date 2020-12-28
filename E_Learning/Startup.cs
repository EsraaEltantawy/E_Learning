using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_Learning.Startup))]
namespace E_Learning
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
