using Microsoft.Extensions.DependencyInjection;
using Jal.ChainOfResponsability;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Jal.ChainOfResponsability.Microsoft.Extensions.DependencyInjection;

namespace Jal.HttpClient.Microsoft.Extensions.DependencyInjection.Installer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClient(this IServiceCollection servicecollection)
        {
            servicecollection.AddChainOfResponsability();

            servicecollection.TryAddSingleton<IHttpFluentHandler, HttpFluentHandler>();

            servicecollection.TryAddSingleton<IHttpHandler, HttpHandler>();

            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, BasicHttpAuthenticatorMiddleware>();

            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, HttpMiddelware>();

            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, TokenAuthenticatorMiddleware>();

            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, MemoryCacheMiddleware>();

            servicecollection.AddSingleton<IAsyncMiddleware<HttpContext>, TracingMiddleware>();

            return servicecollection;
        }
    }
}
