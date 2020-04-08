using System.Threading.Tasks;

namespace Jal.HttpClient
{

    public interface IHttpHandler
    {
        Task<HttpResponse> SendAsync(HttpRequest request);
    }
}