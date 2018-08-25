namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMiddlewareDescriptor
    {
        void Add<THttpMiddlewareType>() where THttpMiddlewareType : IHttpMiddleware;
    }
}