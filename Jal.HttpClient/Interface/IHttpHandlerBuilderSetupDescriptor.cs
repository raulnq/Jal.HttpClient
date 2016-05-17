namespace Jal.HttpClient.Interface
{
    public interface IHttpHandlerBuilderSetupDescriptor
    {
        IHttpHandlerBuilderSetupDescriptor UseHttpHandlerBuilder(IHttpHandlerBuilder httpHandlerBuilder);

        IHttpHandlerBuilder Create();
    }
}