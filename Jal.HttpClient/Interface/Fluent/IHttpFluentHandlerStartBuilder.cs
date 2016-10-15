namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpFluentHandlerStartBuilder
    {
        IHttpFluentHandlerEndBuilder UseHttpHandler(IHttpHandler httpHandler);
    }
}