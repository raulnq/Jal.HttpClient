using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Impl.Fluent;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Installer
{
    public class HttpClientInstaller : IWindsorInstaller
    {
        private readonly int _timeout;

        public HttpClientInstaller(int timeout=5000)
        {
            _timeout = timeout;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IHttpHandler>().ImplementedBy<HttpHandler>(),
                Component.For<IHttpMiddleware>().ImplementedBy<HttpMiddelware>().Named(typeof(HttpMiddelware).FullName),
                Component.For<IHttpMiddlewareFactory>().ImplementedBy<HttpMiddlewareFactory>(),
                Component.For<IHttpRequestToWebRequestConverter>().ImplementedBy<HttpRequestToWebRequestConverter>().DependsOn(new { timeout = _timeout }),
                Component.For<IWebResponseToHttpResponseConverter>().ImplementedBy<WebResponseToHttpResponseConverter>(),
                Component.For<IHttpMethodMapper>().ImplementedBy<HttpMethodMapper>(),
                Component.For<IHttpFluentHandler>().ImplementedBy<HttpFluentHandler>()
                
                );
        }

    }
}