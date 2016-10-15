namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpFluentHandler
    {
        IHttpHandler Handler { get; }

        IHttpHandlerDescriptor Get(string url);

        IHttpHandlerDescriptor Post(string url);

        IHttpHandlerDescriptor Put(string url);

        IHttpHandlerDescriptor Head(string url);

        IHttpHandlerDescriptor Delete(string url);

        IHttpHandlerDescriptor Patch(string url);

        IHttpHandlerDescriptor Options(string url);

    }
}