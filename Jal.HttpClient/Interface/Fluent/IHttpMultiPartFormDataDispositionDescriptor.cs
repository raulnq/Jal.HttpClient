namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMultiPartFormDataDispositionDescriptor
    {
        IHttpMultiPartFormDataContentTypeDescriptor WithDisposition(string name, string filename = "");
    }
}