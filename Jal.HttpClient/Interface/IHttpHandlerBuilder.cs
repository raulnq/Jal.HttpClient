using Jal.HttpClient.Fluent;

namespace Jal.HttpClient.Interface
{
    public interface IHttpHandlerBuilder
    {
        IHttpHandler Handler { get; }

        HttpHandlerDescriptor Get(string url);

        HttpHandlerDescriptor Post(string url);

        HttpHandlerDescriptor Put(string url);

        HttpHandlerDescriptor Head(string url);

        HttpHandlerDescriptor Delete(string url);

        HttpHandlerDescriptor Patch(string url);

        HttpHandlerDescriptor Options(string url);

    }
}