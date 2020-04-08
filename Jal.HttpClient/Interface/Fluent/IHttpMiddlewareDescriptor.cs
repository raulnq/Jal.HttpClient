using Jal.ChainOfResponsability;
using System;

namespace Jal.HttpClient
{
    public interface IHttpMiddlewareDescriptor
    {
        void Add<THttpMiddlewareType>(Action<IHttpContextDescriptor> action=null) where THttpMiddlewareType : IAsyncMiddleware<HttpContext>;
    }
}