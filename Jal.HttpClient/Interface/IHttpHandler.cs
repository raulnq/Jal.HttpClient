using System.Threading.Tasks;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{
    public interface IHttpHandler
    {
        int Timeout { get; set; }

        IHttpInterceptor Interceptor { get; set; }

        IHttpRequestToWebRequestConverter RequestConverter { get; }

        IWebResponseToHttpResponseConverter ResponseConverter { get; }

        HttpResponse Send(HttpRequest httpRequest);

        Task<HttpResponse> SendAsync(HttpRequest httpRequest);
    }
}