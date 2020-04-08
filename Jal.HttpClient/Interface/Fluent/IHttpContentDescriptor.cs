using System.Net.Http;

namespace Jal.HttpClient
{
    public interface IHttpContentDescriptor
    {
        IHttpContentTypeDescriptor WithContent(HttpContent content);
    }
}