using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.HttpClient.Interface;

namespace Jal.HttpClient.Polly.Installer
{
    public class HttpClienPollyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IHttpMiddleware>().ImplementedBy<OnConditionRetryMiddelware>().Named(typeof(OnConditionRetryMiddelware).FullName)
                );
        }

    }
}