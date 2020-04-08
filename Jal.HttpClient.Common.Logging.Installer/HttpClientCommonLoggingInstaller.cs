using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient.Common.Logging.Installer
{
    public class HttpClientCommonLoggingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (!container.Kernel.HasComponent(typeof(CommonLoggingMiddelware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<CommonLoggingMiddelware>().Named(typeof(CommonLoggingMiddelware).FullName));
            } 
        }
    }
}