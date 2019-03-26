using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Impl.Fluent;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;
using LightInject;

namespace Jal.HttpClient.LightInject.Installer
{
    public class HttpClientCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IHttpFluentHandler, HttpFluentHandler>(new PerContainerLifetime());
            RegisterMultiple<HttpMiddelware, IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>>(serviceRegistry, new PerContainerLifetime());
            serviceRegistry.Register<IHttpHandler, HttpHandler>(new PerContainerLifetime());
            RegisterMultiple<BasicHttpAuthenticatorMiddleware, IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>>(serviceRegistry, new PerContainerLifetime());
            RegisterMultiple<TokenAuthenticatorMiddleware, IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>>(serviceRegistry, new PerContainerLifetime());
            RegisterMultiple<MemoryCacheMiddleware, IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>>(serviceRegistry, new PerContainerLifetime());
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
