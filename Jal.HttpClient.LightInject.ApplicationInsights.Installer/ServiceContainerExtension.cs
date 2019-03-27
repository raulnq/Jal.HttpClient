using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.ApplicationInsights;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using LightInject;
using Microsoft.ApplicationInsights;

namespace Jal.HttpClient.LightInject.ApplicationInsights.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterHttpClientApplicationInsights(this IServiceContainer container, string applicationname)
        {
            container.Register<IMiddlewareAsync<HttpWrapper>>(x => new ApplicationInsightsMiddelware(x.GetInstance<TelemetryClient>(), applicationname), typeof(ApplicationInsightsMiddelware).FullName, new PerContainerLifetime());
        }
    }
}
