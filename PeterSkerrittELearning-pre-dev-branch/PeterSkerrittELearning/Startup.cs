using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PeterSkerrittELearning.Startup))]
namespace PeterSkerrittELearning
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
