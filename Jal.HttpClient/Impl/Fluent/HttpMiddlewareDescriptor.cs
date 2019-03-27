using System;
using System.Linq;
using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpMiddlewareDescriptor : IHttpMiddlewareDescriptor
    {
        private readonly HttpRequest _request;

        public HttpMiddlewareDescriptor(HttpRequest request)
        {
            _request = request;
        }

        public void Add<THttpMiddlewareType>(Action<IHttpContextDescriptor> action = null) where THttpMiddlewareType : IMiddlewareAsync<HttpWrapper>
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