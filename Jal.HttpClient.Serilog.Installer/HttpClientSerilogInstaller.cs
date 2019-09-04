using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Serilog.Installer
{
    public class HttpClientSerilogInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMiddlewareAsync<HttpWrapper>>().ImplementedBy<SerilogMiddelware>().Named(typeof(SerilogMiddelware).FullName));
        }
    }
}