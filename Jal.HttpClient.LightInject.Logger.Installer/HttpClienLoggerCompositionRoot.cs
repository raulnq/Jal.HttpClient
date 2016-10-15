using Jal.HttpClient.Interface;
using LightInject;

namespace Jal.HttpClient.Logger.LightInject.Installer
{
    public class HttpClienLoggerCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IHttpInterceptor, HttpLogger>(new PerContainerLifetime());
        }
    }
}
