using System;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMiddlewareDescriptor
    {
        void Add<THttpMiddlewareType>(Action<IHttpContextDescriptor> action=null) where THttpMiddlewareType : IHttpMiddleware;
    }
}