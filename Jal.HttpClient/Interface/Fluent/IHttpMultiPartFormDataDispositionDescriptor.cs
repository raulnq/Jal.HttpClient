namespace Jal.HttpClient
{
    public interface IHttpMultiPartFormDataDispositionDescriptor
    {
        IHttpMultiPartFormDataContentTypeDescriptor WithDisposition(string name, string filename = "");
    }
}