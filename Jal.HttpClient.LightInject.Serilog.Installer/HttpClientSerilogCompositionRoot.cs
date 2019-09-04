using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Serilog;
using Jal.HttpClient.Model;
using LightInject;

namespace Jal.HttpClient.LightInject.Serilog.Installer
{
    public class HttpClientSerilogCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMiddlewareAsync<HttpWrapper>, SerilogMiddelware>(typeof(SerilogMiddelware).FullName, new PerContainerLifetime());
        }
    }
}
