using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Model;

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
            container.Register(Component.For<IMiddlewareAsync<HttpWrapper>>().ImplementedBy<ApplicationInsightsMiddelware>().Named(typeof(ApplicationInsightsMiddelware).FullName).DependsOn(new { applicationname = _applicationname }));
        }
    }
}