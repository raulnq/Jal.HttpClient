using System;
using System.Threading.Tasks;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{
    public interface IHttpMiddleware
    {
        HttpResponse Send(HttpRequest request, Func<HttpResponse> next);

        Task<HttpResponse> SendAsync(HttpRequest request, Func<Task<HttpResponse>> next);
    }
}