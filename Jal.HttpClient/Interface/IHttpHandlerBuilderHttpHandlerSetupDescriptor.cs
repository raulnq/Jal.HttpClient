namespace Jal.HttpClient.Interface
{
    public interface IHttpHandlerBuilderHttpHandlerSetupDescriptor
    {
        IHttpHandlerBuilderSetupDescriptor UseHttpHandler(IHttpHandler httpHandler);
    }
}