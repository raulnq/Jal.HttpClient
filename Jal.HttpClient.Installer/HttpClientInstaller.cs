using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Impl.Fluent;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Installer
{
    public class HttpClientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IHttpHandler>().ImplementedBy<HttpHandler>(),
                Component.For<IMiddlewareAsync<HttpWrapper>>().ImplementedBy<HttpMiddelware>().Named(typeof(HttpMiddelware).FullName),
                Component.For<IHttpFluentHandler>().ImplementedBy<HttpFluentHandler>()
                );

            container.Register(Component.For<IMiddlewareAsync<HttpWrapper>>().ImplementedBy<BasicHttpAuthenticatorMiddleware>().Named(typeof(BasicHttpAuthenticatorMiddleware).FullName));
            container.Register(Component.For<IMiddlewareAsync<HttpWrapper>>().ImplementedBy<TokenAuthenticatorMiddleware>().Named(typeof(TokenAuthenticatorMiddleware).FullName));
            container.Register(Component.For<IMiddlewareAsync<HttpWrapper>>().ImplementedBy<MemoryCacheMiddleware>().Named(typeof(MemoryCacheMiddleware).FullName));
            
        }

    }
}