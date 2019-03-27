using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Common.Logging;
using Jal.HttpClient.Model;
using LightInject;

namespace Jal.HttpClient.LightInject.Common.Logging.Installer
{
    public class HttpClientCommonLoggingCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMiddlewareAsync<HttpWrapper>, CommonLoggingMiddelware>(typeof(CommonLoggingMiddelware).FullName, new PerContainerLifetime());
        }
    }
}
