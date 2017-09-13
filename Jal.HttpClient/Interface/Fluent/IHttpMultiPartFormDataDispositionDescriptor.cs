namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMultiPartFormDataDispositionDescriptor
    {
        IHttpMultiPartFormDataContentTypeDescriptor WithDisposition(string name, string filename = "");
    }

    public interface IHttpMultiPartFormDataContentTypeDescriptor
    {
        IHttpMultiPartFormDataContentTypeDescriptor WithCharacterSet(string characterset);

        IHttpMultiPartFormDataContentTypeDescriptor WithContentType(string contenttype);
    }
}