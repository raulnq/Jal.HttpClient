namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpHandlerBuilder
    {
        IHttpHandlerBuilder UseHttpInterceptor(IHttpInterceptor interceptor);

        IHttpHandlerBuilder WithTimeout(int timeout);

        IHttpHandler Create { get; }
    }
}