using Jal.ChainOfResponsability;
using Jal.HttpClient.Polly;
using LightInject;
using System.Linq;

namespace Jal.HttpClient.LightInject.Polly.Installer
{
    public class HttpClientPollyCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            if (serviceRegistry.AvailableServices.All(x => x.ServiceName != typeof(OnConditionRetryMiddelware).FullName))
            {
                serviceRegistry.Register<IAsyncMiddleware<HttpContext>, OnConditionRetryMiddelware>(typeof(OnConditionRetryMiddelware).FullName, new PerContainerLifetime());
            }

            if (serviceRegistry.AvailableServices.All(x => x.ServiceName != typeof(CircuitBreakerMiddelware).FullName))
            {
                serviceRegistry.Register<IAsyncMiddleware<HttpContext>, CircuitBreakerMiddelware>(typeof(CircuitBreakerMiddelware).FullName, new PerContainerLifetime());
            }

            if (serviceRegistry.AvailableServices.All(x => x.ServiceName != typeof(TimeoutMiddelware).FullName))
            {
                serviceRegistry.Register<IAsyncMiddleware<HttpContext>, TimeoutMiddelware>(typeof(TimeoutMiddelware).FullName, new PerContainerLifetime());
            }
                
            
            
        }
    }
}
