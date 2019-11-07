using Jal.Locator.Microsoft.Extensions.DependencyInjection.Interface;
using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Model;
using Jal.Locator.Microsoft.Extensions.DependencyInjection.Extensions;
using Jal.HttpClient.ApplicationInsights;

namespace Jal.HttpClient.LightInject.ApplicationInsights.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static INamedServiceCollection AddApplicationInsightsForHttpClient(this INamedServiceCollection servicecollection)
        {
            servicecollection.AddSingleton<IMiddlewareAsync<HttpWrapper>, ApplicationInsightsMiddelware>(typeof(ApplicationInsightsMiddelware).FullName);

            return servicecollection;
        }
    }
}
