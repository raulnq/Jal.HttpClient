using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Interface;

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
                Component.For<IHttpHandler>().ImplementedBy<HttpHandler>().DependsOn(new { Timeout = _timeout }),
                Component.For<IHttpRequestToWebRequestConverter>().ImplementedBy<HttpRequestToWebRequestConverter>(),
                Component.For<IWebResponseToHttpResponseConverter>().ImplementedBy<WebResponseToHttpResponseConverter>(),
                Component.For<IHttpMethodMapper>().ImplementedBy<HttpMethodMapper>(),
                Component.For<IHttpContentTypeBuilder>().ImplementedBy<HttpContentTypeBuilder>(),
                Component.For<IHttpHandlerBuilder>().ImplementedBy<HttpHandlerBuilder>()
                
                );
        }

    }
}