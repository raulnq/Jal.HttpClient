using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Jal.ChainOfResponsability.Microsoft.Extensions.DependencyInjection;
using System;

namespace Jal.HttpClient.Microsoft.Extensions.DependencyInjection.Installer
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClient(this IServiceCollection servicecollection, Action<IHttpClientBuilder> action = null)
        {
            servicecollection.AddChainOfResponsability(c=>
            {
                var builder = new HttpClientBuilder(c);

                builder.Add<TracingMiddleware>();

                builder.Add<MemoryCacheMiddleware>();

                builder.Add<TokenAuthenticatorMiddleware>();

                builder.Add<HttpMiddelware>();

                builder.Add<BasicHttpAuthenticatorMiddleware>();

                if (action != null)
                {
                    action(builder);
                }
            });

            servicecollection.TryAddSingleton<IHttpFluentHandler, HttpFluentHandler>();

            servicecollection.TryAddSingleton<IHttpHandler, HttpHandler>();

            return servicecollection;
        }
    }
}
