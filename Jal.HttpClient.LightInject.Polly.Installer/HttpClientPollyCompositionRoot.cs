using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using Jal.HttpClient.Polly;
using LightInject;

namespace Jal.HttpClient.LightInject.Polly.Installer
{
    public class HttpClientPollyCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            RegisterMultiple<OnConditionRetryMiddelware, IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>>(serviceRegistry, new PerContainerLifetime());
        }

        public void RegisterMultiple<TService, TInterface1, TInterface2>(IServiceRegistry container, ILifetime lifetime = null)
        where TService : TInterface1, TInterface2
        {
            container.Register<TService>(lifetime);
            container.Register(f => (TInterface1)f.GetInstance<TService>(), typeof(TService).FullName);
            container.Register(f => (TInterface2)f.GetInstance<TService>(), typeof(TService).FullName);
        }
    }
}
