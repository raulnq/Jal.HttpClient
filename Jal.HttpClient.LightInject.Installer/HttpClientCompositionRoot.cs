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
            serviceRegistry.Register<IMiddlewareAsync<HttpWrapper>, BasicHttpAuthenticatorMiddleware>(typeof(BasicHttpAuthenticatorMiddleware).FullName, new PerContainerLifetime());
            serviceRegistry.Register<IMiddlewareAsync<HttpWrapper>, HttpMiddelware>(typeof(HttpMiddelware).FullName, new PerContainerLifetime());
            serviceRegistry.Register<IMiddlewareAsync<HttpWrapper>, TokenAuthenticatorMiddleware>(typeof(TokenAuthenticatorMiddleware).FullName, new PerContainerLifetime());
            serviceRegistry.Register<IMiddlewareAsync<HttpWrapper>, MemoryCacheMiddleware>(typeof(MemoryCacheMiddleware).FullName, new PerContainerLifetime());
            serviceRegistry.Register<IHttpHandler, HttpHandler>(new PerContainerLifetime());
        }
    }
}
