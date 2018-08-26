using System;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMiddlewareDescriptor
    {
        void Add<THttpMiddlewareType>(Action<IHttpDataDescriptor> action=null) where THttpMiddlewareType : IHttpMiddleware;
    }
}