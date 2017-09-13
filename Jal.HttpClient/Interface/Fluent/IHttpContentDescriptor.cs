using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpContentDescriptor
    {
        IHttpContentTypeDescriptor WithContent(HttpRequestContent requestContent);
    }
}