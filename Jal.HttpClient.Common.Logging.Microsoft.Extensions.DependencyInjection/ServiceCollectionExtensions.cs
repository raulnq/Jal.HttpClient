using Jal.ChainOfResponsability;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jal.HttpClient.Common.Logging.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonLoggingForHttpClient(this IServiceCollection servicecollection)
        {
            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, CommonLoggingMiddelware>();

            return servicecollection;
        }
    }
}
