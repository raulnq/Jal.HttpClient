namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpHandlerBuilderStartFluentBuilder
    {
        IHttpHandlerBuilderEndFluentBuilder UseHttpHandler(IHttpHandler httpHandler);

        IHttpHandlerBuilderEndFluentBuilder UseHttpHandlerBuilder(IHttpHandlerBuilder httpHandlerBuilder);
    }
}