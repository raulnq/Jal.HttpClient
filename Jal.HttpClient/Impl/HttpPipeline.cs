using System;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpPipeline : IHttpPipeline
    {
        private readonly IHttpMiddlewareFactory _factory;

        public HttpPipeline(IHttpMiddlewareFactory factory)
        {
            _factory = factory;
        }

        public HttpResponse Send(HttpRequest request, Type[] MiddelwareTypes)
        {
            return GetNext().Invoke(request, new HttpContext() {  Index = 0, MiddlewareTypes = MiddelwareTypes });
        }

        private Func<HttpRequest, HttpContext, HttpResponse> GetNext()
        {
            return (r,c) =>
            {
                if (c.Index < c.MiddlewareTypes.Length)
                {
                    var middleware = _factory.Create(c.MiddlewareTypes[c.Index]);
                    c.Index++;
                    return middleware.Send(r, GetNext(), c);
                }
                else
                {
                    var middleware = _factory.Create(c.MiddlewareTypes[c.MiddlewareTypes.Length - 1]);
                    return middleware.Send(r, GetNext(), c);
                }
            };
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request, Type[] MiddelwareTypes)
        {
            return await GetNextAsync().Invoke(request, new HttpContext() { Index = 0, MiddlewareTypes = MiddelwareTypes });
        }

        private Func<HttpRequest, HttpContext, Task<HttpResponse>> GetNextAsync()
        {
            return (r,c) =>
            {
                if (c.Index < c.MiddlewareTypes.Length)
                {
                    var middleware = _factory.Create(c.MiddlewareTypes[c.Index]);
                    c.Index++;
                    return middleware.SendAsync(r, GetNextAsync(),c);
                }
                else
                {
                    var middleware = _factory.Create(c.MiddlewareTypes[c.MiddlewareTypes.Length-1]);
                    return middleware.SendAsync(r, GetNextAsync(),c);
                }
            };
        }
    }
}