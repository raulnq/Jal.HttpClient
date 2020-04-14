using Jal.ChainOfResponsability.LightInject.Installer;
using LightInject;
using System;

namespace Jal.HttpClient.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddHttpClient(this IServiceContainer container, Action<IHttpClientBuilder> action = null)
        {
            container.AddChainOfResponsability(c=>
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

            container.RegisterFrom<HttpClientCompositionRoot>();
        }

        public static IHttpFluentHandler GetHttpClient(this IServiceContainer container)
        {
            return container.GetInstance<IHttpFluentHandler>();
        }
    }
}
