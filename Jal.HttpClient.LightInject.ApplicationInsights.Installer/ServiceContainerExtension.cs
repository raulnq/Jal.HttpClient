using Jal.ChainOfResponsability;
using Jal.HttpClient.ApplicationInsights;
using LightInject;
using Microsoft.ApplicationInsights;
using System.Linq;

namespace Jal.HttpClient.LightInject.ApplicationInsights.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddApplicationInsightsForHttpClient(this IServiceContainer container, string applicationname)
        {
            if (container.AvailableServices.All(x => x.ServiceName != typeof(ApplicationInsightsMiddelware).FullName))
            {
                container.Register<IAsyncMiddleware<HttpContext>>(x => new ApplicationInsightsMiddelware(x.GetInstance<TelemetryClient>(), applicationname), typeof(ApplicationInsightsMiddelware).FullName, new PerContainerLifetime());
            }   
        }
    }
}
