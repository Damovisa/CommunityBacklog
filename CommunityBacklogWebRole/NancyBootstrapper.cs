using System.Web.Routing;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Session;
using Nancy.TinyIoc;

namespace CommunityBacklogWebRole
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            CookieBasedSessions.Enable(pipelines);
            StaticConfiguration.DisableErrorTraces = false;
           
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.AddDirectory("Scripts", "Scripts");
        }
    }
}