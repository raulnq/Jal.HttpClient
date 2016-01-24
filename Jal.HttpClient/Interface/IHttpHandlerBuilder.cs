using Jal.HttpClient.Fluent;
using Jal.HttpClient.Impl;

namespace Jal.HttpClient.Interface
{
    public interface IHttpHandlerBuilder
    {
        HttpHandlerDescriptor For(string url);
    }
}