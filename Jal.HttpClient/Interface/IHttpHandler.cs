using System.Threading.Tasks;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{

    public interface IHttpHandler
    {
        HttpResponse Send(HttpRequest httpRequest);

        Task<HttpResponse> SendAsync(HttpRequest httpRequest);
    }
}