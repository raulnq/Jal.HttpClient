using Jal.HttpClient.Common.Logging;
using Jal.HttpClient.Interface;
using LightInject;

namespace Jal.HttpClient.LightInject.Common.Logging.Installer
{
    public class HttpClienCommonLoggingCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IHttpMiddleware, CommonLoggingMiddelware>(typeof(CommonLoggingMiddelware).FullName, new PerContainerLifetime());
        }
    }
}
