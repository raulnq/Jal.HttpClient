using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{
    public interface IHttpContentTypeBuilder
    {
        string Build(string httpContentType, string httpCharacterSet);
    }
}
