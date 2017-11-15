using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Polls.Startup))]
namespace Polls
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
