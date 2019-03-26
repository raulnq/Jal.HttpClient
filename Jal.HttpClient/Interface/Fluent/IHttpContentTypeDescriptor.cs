namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpContentTypeDescriptor : IHttpSenderDescriptor
    {
        IHttpContentTypeDescriptor WithEncoding(string encoding);
       
        IHttpContentTypeDescriptor WithContentType(string contenttype);
    }
}