using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VideoStore.WebClient.Startup))]
namespace VideoStore.WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
       
        }
    }
}
