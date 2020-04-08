namespace Jal.HttpClient
{
    public interface IHttpContentTypeDescriptor : IHttpSenderDescriptor
    {
        IHttpContentTypeDescriptor WithContentType(string contenttype);
    }
}