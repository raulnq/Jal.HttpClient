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
            container.Register<ApplicationInsightsMiddelware>(x => new ApplicationInsightsMiddelware(x.GetInstance<TelemetryClient>(), applicationname), new PerContainerLifetime());
            RegisterMultiple<ApplicationInsightsMiddelware, IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>>(container, new PerContainerLifetime());
        }

        public static void RegisterMultiple<TService, TInterface1, TInterface2>(IServiceContainer container, ILifetime lifetime = null)
        where TService : TInterface1, TInterface2
        {
            container.Register(f => (TInterface1)f.GetInstance<TService>(), typeof(TService).FullName);
            container.Register(f => (TInterface2)f.GetInstance<TService>(), typeof(TService).FullName);
        }
    }
}
