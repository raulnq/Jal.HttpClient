using System;
using System.Linq;
using Jal.HttpClient.Interface;

namespace Jal.HttpClient.Impl
{
    public class HttpMiddlewareFactory : IHttpMiddlewareFactory
    {
        private readonly IHttpMiddleware[] _middlewares;

        public HttpMiddlewareFactory(IHttpMiddleware[] middlewares)
        {
            _middlewares = middlewares;
        }
        public IHttpMiddleware Create(Type type)
        {
            return _middlewares.First(x => x.GetType() == type); 
        }
    }
}