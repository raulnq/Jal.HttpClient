using Castle.Windsor;
using Jal.ChainOfResponsability.Installer;
using System;

namespace Jal.HttpClient.Installer
{

    public static class WindsorContainerExtensions
    {
        public static void AddHttpClient(this IWindsorContainer container, Action<IHttpClientBuilder> action = null)
        {
            container.AddChainOfResponsability(c =>
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

            container.Install(new HttpClientInstaller());
        }

        public static IHttpFluentHandler GetHttpClient(this IWindsorContainer container)
        {
            return container.Resolve<IHttpFluentHandler>();
        }
    }
}