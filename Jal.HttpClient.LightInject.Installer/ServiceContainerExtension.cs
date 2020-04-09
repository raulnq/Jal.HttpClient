using Jal.ChainOfResponsability.LightInject.Installer;
using LightInject;
using System;

namespace Jal.HttpClient.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddHttpClient(this IServiceContainer container, Action<IServiceContainer> action = null)
        {
            container.AddChainOfResponsability();

            container.RegisterFrom<HttpClientCompositionRoot>();

            if (action != null)
            {
                action(container);
            }
        }

        public static IHttpFluentHandler GetHttpClient(this IServiceContainer container)
        {
            return container.GetInstance<IHttpFluentHandler>();
        }
    }
}
