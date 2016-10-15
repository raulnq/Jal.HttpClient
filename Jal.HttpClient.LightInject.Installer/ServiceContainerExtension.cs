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
            container.Register<IHttpRequestToWebRequestConverter, HttpRequestToWebRequestConverter>(new PerContainerLifetime());
            container.Register<IWebResponseToHttpResponseConverter, WebResponseToHttpResponseConverter>(new PerContainerLifetime());
            container.Register<IHttpMethodMapper, HttpMethodMapper>(new PerContainerLifetime());
            container.Register<IHttpFluentHandler, HttpFluentHandler>(new PerContainerLifetime());
            container.Register<IHttpHandler>(factory => new HttpHandler(factory.GetInstance<IHttpRequestToWebRequestConverter>(), factory.GetInstance<IWebResponseToHttpResponseConverter>()) { Timeout = timeout }, new PerContainerLifetime());
        }
    }
}
