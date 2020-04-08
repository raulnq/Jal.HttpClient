using Jal.ChainOfResponsability;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jal.HttpClient.ApplicationInsights.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationInsightsForHttpClient(this IServiceCollection servicecollection)
        {
            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, ApplicationInsightsMiddelware>();

            return servicecollection;
        }
    }
}
