using Jal.HttpClient.Impl;
using Jal.HttpClient.Impl.Fluent;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using LightInject;

namespace Jal.HttpClient.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterHttpClient(this IServiceContainer container, int timeout = 5000)
        {
            //container.Register<IHttpRequestToWebRequestConverter>(x=> new HttpRequestToWebRequestConverter(x.GetInstance<IHttpMethodMapper>(), timeout) , new PerContainerLifetime());
            //container.Register<IWebResponseToHttpResponseConverter, WebResponseToHttpResponseConverter>(new PerContainerLifetime());
            //container.Register<IHttpMethodMapper, HttpMethodMapper>(new PerContainerLifetime());
            container.Register<IHttpFluentHandler, HttpFluentHandler>(new PerContainerLifetime());
            container.Register<IHttpMiddleware, HttpMiddelware>(typeof(HttpMiddelware).FullName, new PerContainerLifetime());
            container.Register<IHttpMiddlewareFactory, HttpMiddlewareFactory>(new PerContainerLifetime());
            container.Register<IHttpHandler, HttpHandler>(new PerContainerLifetime());
            container.Register<IHttpPipeline, HttpPipeline>(new PerContainerLifetime());
            container.Register<IHttpMiddleware, BasicHttpAuthenticatorMiddleware>(typeof(BasicHttpAuthenticatorMiddleware).FullName, new PerContainerLifetime());
            container.Register<IHttpMiddleware, TokenAuthenticatorMiddleware>(typeof(TokenAuthenticatorMiddleware).FullName, new PerContainerLifetime());
            container.Register<IHttpMiddleware, MemoryCacheMiddleware>(typeof(MemoryCacheMiddleware).FullName, new PerContainerLifetime());
            
        }
    }
}
