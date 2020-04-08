using LightInject;

namespace Jal.HttpClient.LightInject.Polly.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddPollyForHttpClient(this IServiceContainer container)
        {
            container.RegisterFrom<HttpClientPollyCompositionRoot>();
        }
    }
}
