using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{
    public interface IHttpContentTypeMapper
    {
        string Map(HttpContentType httpContentType);
    }
}
