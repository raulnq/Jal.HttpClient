using System.Threading.Tasks;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{

    public interface IHttpHandler
    {
        Task<HttpResponse> SendAsync(HttpRequest request);
    }
}