using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient.Serilog.Installer
{
    public class HttpClientSerilogInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (!container.Kernel.HasComponent(typeof(SerilogMiddelware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<SerilogMiddelware>().Named(typeof(SerilogMiddelware).FullName));
            }  
        }
    }
}