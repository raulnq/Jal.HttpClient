using Jal.ChainOfResponsability;
using Jal.HttpClient.Common.Logging;
using LightInject;
using System.Linq;

namespace Jal.HttpClient.LightInject.Common.Logging.Installer
{
    public class HttpClientCommonLoggingCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            if (serviceRegistry.AvailableServices.All(x => x.ServiceName != typeof(CommonLoggingMiddelware).FullName))
            {
                serviceRegistry.Register<IAsyncMiddleware<HttpContext>, CommonLoggingMiddelware>(typeof(CommonLoggingMiddelware).FullName, new PerContainerLifetime());
            }
                
        }
    }
}
