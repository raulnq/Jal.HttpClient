using System;

namespace Jal.HttpClient
{
    public class HttpBuilderContext
    {
        public HttpRequest Request { get; }

        public IHttpHandler Handler { get; }

        public Action<IHttpQueryParameterDescriptor> QueryParemeterDescriptorAction { get; set; }

        public Action<IHttpHeaderDescriptor> HeaderDescriptorAction { get; set; }

        public Action<IHttpMiddlewareDescriptor> MiddlewareDescriptorAction { get; set; }

        public HttpBuilderContext(HttpRequest request, IHttpHandler handler)
        {
            Request = request;
            Handler = handler;
        }
    }
}