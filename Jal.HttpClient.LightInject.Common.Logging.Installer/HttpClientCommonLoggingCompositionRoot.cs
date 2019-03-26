using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Common.Logging;
using Jal.HttpClient.Model;
using LightInject;

namespace Jal.HttpClient.LightInject.Common.Logging.Installer
{
    public class HttpClientCommonLoggingCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            RegisterMultiple<CommonLoggingMiddelware, IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>>(serviceRegistry, new PerContainerLifetime());
        }

        public static void RegisterMultiple<TService, TInterface1, TInterface2>(IServiceRegistry container, ILifetime lifetime = null)
        where TService : TInterface1, TInterface2
        {
            container.Register<TService>(lifetime);
            container.Register(f => (TInterface1)f.GetInstance<TService>(), typeof(TService).FullName);
            container.Register(f => (TInterface2)f.GetInstance<TService>(), typeof(TService).FullName);
        }
    }
}
