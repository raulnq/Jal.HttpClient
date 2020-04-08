using LightInject;

namespace Jal.HttpClient.LightInject.Serilog.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddSerilogForHttpClient(this IServiceContainer container)
        {
            container.RegisterFrom<HttpClientSerilogCompositionRoot>();
        }
    }
}
