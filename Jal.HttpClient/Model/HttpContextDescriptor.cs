using System;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Model
{
    public class HttpContextDescriptor
    {
        public HttpRequest Request { get; }

        public IHttpHandler Handler { get; }

        public Action<IHttpQueryParameterDescriptor> QueryParemeterDescriptorAction { get; set; }

        public Action<IHttpHeaderDescriptor> HeaderDescriptorAction { get; set; }

        public Action<IHttpMiddlewareDescriptor> MiddlewareDescriptorAction { get; set; }

        public HttpContextDescriptor(HttpRequest request, IHttpHandler handler)
        {
            Request = request;
            Handler = handler;
        }
    }
}