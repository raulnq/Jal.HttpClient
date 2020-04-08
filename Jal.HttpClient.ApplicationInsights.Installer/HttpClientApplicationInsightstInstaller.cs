using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient.ApplicationInsights.Installer
{
    public class HttpClientApplicationInsightstInstaller : IWindsorInstaller
    {
        private readonly string _applicationname;

        public HttpClientApplicationInsightstInstaller(string applicationname)
        {
            _applicationname = applicationname;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (!container.Kernel.HasComponent(typeof(ApplicationInsightsMiddelware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<ApplicationInsightsMiddelware>().Named(typeof(ApplicationInsightsMiddelware).FullName).DependsOn(new { applicationname = _applicationname }));
            }
        }
    }
}