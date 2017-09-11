using System;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Model
{
    public class HttpContext
    {
        public HttpRequest HttpRequest { get; set; }

        public IHttpHandler HttpHandler { get; set; }

        public Action<IHttpQueryParameterDescriptor> QueryParemeterDescriptorAction { get; set; }

        public Action<IHttpHeaderDescriptor> HeaderDescriptorAction { get; set; }

        public HttpContext(HttpRequest httprequest, IHttpHandler httphandler)
        {
            HttpRequest = httprequest;
            HttpHandler = httphandler;
        }
    }
}