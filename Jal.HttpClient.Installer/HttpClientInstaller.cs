using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Interface;

namespace Jal.HttpClient.Installer
{
    public class HttpClientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IHttpHandler>().ImplementedBy<HttpHandler>(),
                Component.For<IHttpRequestToWebRequestConverter>().ImplementedBy<HttpRequestToWebRequestConverter>(),
                Component.For<IWebResponseToHttpResponseConverter>().ImplementedBy<WebResponseToHttpResponseConverter>(),
                Component.For<IHttpMethodMapper>().ImplementedBy<HttpMethodMapper>(),
                Component.For<IHttpContentTypeMapper>().ImplementedBy<HttpContentTypeMapper>()
                );
        }

    }
}