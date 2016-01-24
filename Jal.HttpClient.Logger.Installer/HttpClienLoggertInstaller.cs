using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.HttpClient.Interface;

namespace Jal.HttpClient.Logger.Installer
{
    public class HttpClienLoggertInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IHttpInterceptor>().ImplementedBy<HttpLogger>()
                );
        }

    }
}