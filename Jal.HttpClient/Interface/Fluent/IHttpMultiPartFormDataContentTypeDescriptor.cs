namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMultiPartFormDataContentTypeDescriptor
    {
        IHttpMultiPartFormDataContentTypeDescriptor WithEncoding(string encoding);

        IHttpMultiPartFormDataContentTypeDescriptor WithContentType(string contenttype);
    }
}