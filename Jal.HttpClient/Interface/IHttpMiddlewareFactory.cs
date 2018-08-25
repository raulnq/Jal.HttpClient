using System;

namespace Jal.HttpClient.Interface
{
    public interface IHttpMiddlewareFactory
    {
        IHttpMiddleware Create(Type type);
    }
}