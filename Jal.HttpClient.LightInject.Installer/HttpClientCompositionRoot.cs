using Jal.ChainOfResponsability;
using LightInject;
using System.Linq;

namespace Jal.HttpClient.LightInject.Installer
{
    public class HttpClientCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            if (serviceRegistry.AvailableServices.All(x => x.ServiceType != typeof(HttpHandler)))
            {
                serviceRegistry.Register<IHttpHandler, HttpHandler>(new PerContainerLifetime());
            }

            if (serviceRegistry.AvailableServices.All(x => x.ServiceType != typeof(HttpFluentHandler)))
            {
                serviceRegistry.Register<IHttpFluentHandler, HttpFluentHandler>(new PerContainerLifetime());
            }

            if (serviceRegistry.AvailableServices.All(x => x.ServiceName != typeof(BasicHttpAuthenticatorMiddleware).FullName))
            {
                serviceRegistry.Register<IAsyncMiddleware<HttpContext>, BasicHttpAuthenticatorMiddleware>(typeof(BasicHttpAuthenticatorMiddleware).FullName, new PerContainerLifetime());
            }

            if (serviceRegistry.AvailableServices.All(x => x.ServiceName != typeof(HttpMiddelware).FullName))
            {
                serviceRegistry.Register<IAsyncMiddleware<HttpContext>, HttpMiddelware>(typeof(HttpMiddelware).FullName, new PerContainerLifetime());
            }

            if (serviceRegistry.AvailableServices.All(x => x.ServiceName != typeof(TokenAuthenticatorMiddleware).FullName))
            {
                serviceRegistry.Register<IAsyncMiddleware<HttpContext>, TokenAuthenticatorMiddleware>(typeof(TokenAuthenticatorMiddleware).FullName, new PerContainerLifetime());
            }

            if (serviceRegistry.AvailableServices.All(x => x.ServiceName != typeof(MemoryCacheMiddleware).FullName))
            {
                serviceRegistry.Register<IAsyncMiddleware<HttpContext>, MemoryCacheMiddleware>(typeof(MemoryCacheMiddleware).FullName, new PerContainerLifetime());
            }

            if (serviceRegistry.AvailableServices.All(x => x.ServiceName != typeof(TracingMiddleware).FullName))
            {
                serviceRegistry.Register<IAsyncMiddleware<HttpContext>, TracingMiddleware>(typeof(TracingMiddleware).FullName, new PerContainerLifetime());
            }
                
            
            
            
            
        }
    }
}
