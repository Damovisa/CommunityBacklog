using CommunityBacklogWebRole;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Nancy.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace CommunityBacklogWebRole
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Map the hubs first, otherwise Nancy grabs them and you get a 404.
            var configuration = new HubConfiguration { EnableDetailedErrors = true };
            app.MapSignalR(configuration);

            var options = new NancyOptions
            {
                Bootstrapper = new NancyBootstrapper()
            };
            app.UseNancy(options);
        }
    }
}