using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Model;
using Jal.HttpClient.Polly;
using LightInject;

namespace Jal.HttpClient.LightInject.Polly.Installer
{
    public class HttpClientPollyCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMiddlewareAsync<HttpWrapper>, OnConditionRetryMiddelware>(typeof(OnConditionRetryMiddelware).FullName, new PerContainerLifetime());
        }
    }
}
