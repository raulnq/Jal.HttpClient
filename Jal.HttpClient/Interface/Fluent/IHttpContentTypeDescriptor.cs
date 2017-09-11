namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpContentTypeDescriptor : IHttpSenderDescriptor
    {
        IHttpContentTypeDescriptor WithCharacterSet(string characterset);
       
        IHttpContentTypeDescriptor WithContentType(string contenttype);
    }
}