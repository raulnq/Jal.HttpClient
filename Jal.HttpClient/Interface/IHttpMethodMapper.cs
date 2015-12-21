using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{
    public interface IHttpMethodMapper
    {
        string Map(HttpMethod httpMethod);
    }
}
