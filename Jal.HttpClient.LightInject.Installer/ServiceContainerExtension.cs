using Jal.ChainOfResponsability.LightInject.Installer;
using LightInject;

namespace Jal.HttpClient.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddHttpClient(this IServiceContainer container)
        {
            container.AddChainOfResponsability();

            container.RegisterFrom<HttpClientCompositionRoot>();
        }
    }
}
