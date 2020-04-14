using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Jal.HttpClient.Installer
{
    public class HttpClientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (!container.Kernel.HasComponent(typeof(IHttpHandler)))
            {
                container.Register(Component.For<IHttpHandler>().ImplementedBy<HttpHandler>());
            }

            if (!container.Kernel.HasComponent(typeof(IHttpFluentHandler)))
            {
                container.Register(Component.For<IHttpFluentHandler>().ImplementedBy<HttpFluentHandler>());
            }
        }
    }
}