using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient.Polly.Installer
{
    public class HttpClientPollyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (!container.Kernel.HasComponent(typeof(OnConditionRetryMiddelware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<OnConditionRetryMiddelware>().Named(typeof(OnConditionRetryMiddelware).FullName));
            }

            if (!container.Kernel.HasComponent(typeof(CircuitBreakerMiddelware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<CircuitBreakerMiddelware>().Named(typeof(CircuitBreakerMiddelware).FullName));
            }

            if (!container.Kernel.HasComponent(typeof(TimeoutMiddelware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<TimeoutMiddelware>().Named(typeof(TimeoutMiddelware).FullName));
            }
        }
    }
}