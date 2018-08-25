using Jal.HttpClient.ApplicationInsights;
using Jal.HttpClient.Interface;
using LightInject;
using Microsoft.ApplicationInsights;

namespace Jal.HttpClient.LightInject.ApplicationInsights.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterHttpClientApplicationInsights(this IServiceContainer container, string applicationname)
        {
            container.Register<IHttpMiddleware>(x => new ApplicationInsightsMiddelware(x.GetInstance<TelemetryClient>(), applicationname), new PerContainerLifetime());
        }
    }
}
