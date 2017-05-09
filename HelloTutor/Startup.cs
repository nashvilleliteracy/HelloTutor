using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HelloTutor.Startup))]
namespace HelloTutor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
