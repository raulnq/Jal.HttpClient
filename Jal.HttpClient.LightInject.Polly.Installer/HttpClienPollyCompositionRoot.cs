using Jal.HttpClient.Interface;
using Jal.HttpClient.Polly;
using LightInject;

namespace Jal.HttpClient.LightInject.Polly.Installer
{
    public class HttpClienPollyCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IHttpMiddleware, OnConditionRetryMiddelware>(typeof(OnConditionRetryMiddelware).FullName, new PerContainerLifetime());
        }
    }
}
