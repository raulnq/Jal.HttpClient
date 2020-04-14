using LightInject;
using System.Linq;

namespace Jal.HttpClient.LightInject.Installer
{
    public class HttpClientCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            if (serviceRegistry.AvailableServices.All(x => x.ServiceType != typeof(IHttpHandler)))
            {
                serviceRegistry.Register<IHttpHandler, HttpHandler>(new PerContainerLifetime());
            }

            if (serviceRegistry.AvailableServices.All(x => x.ServiceType != typeof(IHttpFluentHandler)))
            {
                serviceRegistry.Register<IHttpFluentHandler, HttpFluentHandler>(new PerContainerLifetime());
            }
        }
    }
}
