using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Model;
using System;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMiddlewareDescriptor
    {
        void Add<THttpMiddlewareType>(Action<IHttpContextDescriptor> action=null) where THttpMiddlewareType : IMiddlewareAsync<HttpWrapper>;
    }
}