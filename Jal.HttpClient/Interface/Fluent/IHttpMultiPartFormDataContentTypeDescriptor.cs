namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMultiPartFormDataContentTypeDescriptor
    {
        IHttpMultiPartFormDataContentTypeDescriptor WithCharacterSet(string characterset);

        IHttpMultiPartFormDataContentTypeDescriptor WithDisposition(string name, string filename = "");

        IHttpMultiPartFormDataContentTypeDescriptor WithContentType(string contenttype);


    }
}