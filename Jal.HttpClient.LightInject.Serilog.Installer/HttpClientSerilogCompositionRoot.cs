using Jal.ChainOfResponsability;
using Jal.HttpClient.Serilog;
using LightInject;
using System.Linq;

namespace Jal.HttpClient.LightInject.Serilog.Installer
{
    public class HttpClientSerilogCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            if (serviceRegistry.AvailableServices.All(x => x.ServiceName != typeof(SerilogMiddelware).FullName))
            {
                serviceRegistry.Register<IAsyncMiddleware<HttpContext>, SerilogMiddelware>(typeof(SerilogMiddelware).FullName, new PerContainerLifetime());
            }
                
        }
    }
}
