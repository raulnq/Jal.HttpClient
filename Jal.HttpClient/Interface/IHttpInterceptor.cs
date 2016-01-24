using System.Net;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{
    public interface IHttpInterceptor
    {
        void OnEntry(HttpRequest request);

        void OnExit(HttpResponse response, HttpRequest request);

        void OnSuccess(HttpResponse response, HttpRequest request);

        void OnError(HttpResponse response, HttpRequest request, WebException wex);
    }
}
