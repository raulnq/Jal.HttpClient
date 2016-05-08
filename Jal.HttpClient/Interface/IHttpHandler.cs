using System.Threading.Tasks;
using HttpRequest = Jal.HttpClient.Model.HttpRequest;
using HttpResponse = Jal.HttpClient.Model.HttpResponse;

namespace Jal.HttpClient.Interface
{
    public interface IHttpHandler
    {
        int Timeout { get; set; }

        IHttpInterceptor HttpInterceptor { get; set; }

        HttpResponse Send(HttpRequest httpRequest);

        Task<HttpResponse> SendAsync(HttpRequest httpRequest);
    }
}