using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Common.Logging.Installer
{
    public class HttpClientCommonLoggingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>>().ImplementedBy<CommonLoggingMiddelware>().Named(typeof(CommonLoggingMiddelware).FullName));
        }
    }
}