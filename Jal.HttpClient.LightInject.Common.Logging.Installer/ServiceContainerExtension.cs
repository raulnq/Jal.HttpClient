using LightInject;

namespace Jal.HttpClient.LightInject.Common.Logging.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddCommonLoggingForHttpClient(this IServiceContainer container)
        {
            container.RegisterFrom<HttpClientCommonLoggingCompositionRoot>();
        }
    }
}
