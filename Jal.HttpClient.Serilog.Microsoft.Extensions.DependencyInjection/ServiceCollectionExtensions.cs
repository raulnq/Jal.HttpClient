using Jal.ChainOfResponsability;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jal.HttpClient.Serilog.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilogForHttpClient(this IServiceCollection servicecollection)
        {
            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, SerilogMiddelware>();

            return servicecollection;
        }
    }
}
