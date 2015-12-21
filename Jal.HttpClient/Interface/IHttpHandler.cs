using HttpRequest = Jal.HttpClient.Model.HttpRequest;
using HttpResponse = Jal.HttpClient.Model.HttpResponse;

namespace Jal.HttpClient.Interface
{
    public interface IHttpHandler
    {
        HttpResponse Send(HttpRequest httpRequest);
    }
}