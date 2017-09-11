using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMultiPartFormDataContentDescriptor
    {
        IHttpMultiPartFormDataContentTypeDescriptor WithContent(HttpContent content);
    }
}