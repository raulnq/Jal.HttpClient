namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpFluentHandler
    {
        IHttpHandler Handler { get; }

        IHttpDescriptor Get(string url);

        IHttpDescriptor Post(string url);

        IHttpDescriptor Put(string url);

        IHttpDescriptor Head(string url);

        IHttpDescriptor Delete(string url);

        IHttpDescriptor Patch(string url);

        IHttpDescriptor Options(string url);

    }
}