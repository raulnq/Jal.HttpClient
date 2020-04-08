using Jal.ChainOfResponsability;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jal.HttpClient.Polly.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPollyForHttpClient(this IServiceCollection servicecollection)
        {
            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, OnConditionRetryMiddelware>();

            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, CircuitBreakerMiddelware>();

            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, TimeoutMiddelware>();

            return servicecollection;
        }
    }
}
