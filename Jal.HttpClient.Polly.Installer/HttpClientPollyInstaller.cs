using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Polly.Installer
{
    public class HttpClientPollyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMiddlewareAsync<HttpWrapper>>().ImplementedBy<OnConditionRetryMiddelware>().Named(typeof(OnConditionRetryMiddelware).FullName));
            container.Register(Component.For<IMiddlewareAsync<HttpWrapper>>().ImplementedBy<CircuitBreakerMiddelware>().Named(typeof(CircuitBreakerMiddelware).FullName));
        }
    }
}