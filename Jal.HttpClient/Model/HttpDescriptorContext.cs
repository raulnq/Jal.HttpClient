using System;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Model
{
    public class HttpDescriptorContext
    {
        public HttpRequest HttpRequest { get; set; }

        public IHttpHandler HttpHandler { get; set; }

        public Action<IHttpQueryParameterDescriptor> QueryParemeterDescriptorAction { get; set; }

        public Action<IHttpHeaderDescriptor> HeaderDescriptorAction { get; set; }

        public Action<IHttpMiddlewareDescriptor> MiddlewareDescriptorAction { get; set; }

        public Action<IHttpDataDescriptor> DataDescriptorAction { get; set; }

        public HttpDescriptorContext(HttpRequest httprequest, IHttpHandler httphandler)
        {
            HttpRequest = httprequest;
            HttpHandler = httphandler;
        }
    }
}