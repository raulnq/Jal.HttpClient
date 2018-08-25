using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.HttpClient.Interface;

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
            container.Register(
                Component.For<IHttpMiddleware>().ImplementedBy<ApplicationInsightsMiddelware>().Named(typeof(ApplicationInsightsMiddelware).FullName).DependsOn(new { applicationname = _applicationname })
                );
        }

    }
}