using System;
using System.Linq;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient
{
    public class HttpMiddlewareDescriptor : IHttpMiddlewareDescriptor
    {
        private readonly HttpRequest _request;

        public HttpMiddlewareDescriptor(HttpRequest request)
        {
            _request = request;
        }

        public void Add<THttpMiddlewareType>(Action<IHttpContextDescriptor> action = null) where THttpMiddlewareType : IAsyncMiddleware<HttpContext>
        {
            var type = typeof(THttpMiddlewareType);
            var item = _request.MiddlewareTypes.FirstOrDefault(x => x == type);
            if (item == null)
            {
                _request.MiddlewareTypes.Add(type);
                if (action != null)
                {
                    var descriptor = new HttpContextDescriptor(_request);
                    action(descriptor);
                }
            }
        }
    }
}