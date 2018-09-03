using Jal.HttpClient.Interface;
using Jal.HttpClient.Polly;
using LightInject;

namespace Jal.HttpClient.LightInject.Polly.Installer
{
    public class HttpClientPollyCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IHttpMiddleware, OnConditionRetryMiddelware>(typeof(OnConditionRetryMiddelware).FullName, new PerContainerLifetime());
        }
    }
}
