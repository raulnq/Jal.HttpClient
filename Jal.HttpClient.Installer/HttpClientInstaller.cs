using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability;
using Jal.ChainOfResponsability.Installer;

namespace Jal.HttpClient.Installer
{
    public class HttpClientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddChainOfResponsability();

            if (!container.Kernel.HasComponent(typeof(IHttpHandler)))
            {
                container.Register(Component.For<IHttpHandler>().ImplementedBy<HttpHandler>());
            }

            if (!container.Kernel.HasComponent(typeof(IHttpFluentHandler)))
            {
                container.Register(Component.For<IHttpFluentHandler>().ImplementedBy<HttpFluentHandler>());
            }

            if (!container.Kernel.HasComponent(typeof(HttpMiddelware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<HttpMiddelware>().Named(typeof(HttpMiddelware).FullName));
            }

            if (!container.Kernel.HasComponent(typeof(BasicHttpAuthenticatorMiddleware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<BasicHttpAuthenticatorMiddleware>().Named(typeof(BasicHttpAuthenticatorMiddleware).FullName));
            }

            if (!container.Kernel.HasComponent(typeof(TokenAuthenticatorMiddleware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<TokenAuthenticatorMiddleware>().Named(typeof(TokenAuthenticatorMiddleware).FullName));
            }

            if (!container.Kernel.HasComponent(typeof(MemoryCacheMiddleware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<MemoryCacheMiddleware>().Named(typeof(MemoryCacheMiddleware).FullName));
            }

            if (!container.Kernel.HasComponent(typeof(TracingMiddleware).FullName))
            {
                container.Register(Component.For<IAsyncMiddleware<HttpContext>>().ImplementedBy<TracingMiddleware>().Named(typeof(TracingMiddleware).FullName));
            }
        }

    }
}