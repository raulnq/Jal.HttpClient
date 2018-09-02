using System;
using System.Threading.Tasks;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{
    public interface IHttpMiddleware
    {
        HttpResponse Send(HttpRequest request, Func<HttpRequest, HttpContext, HttpResponse> next, HttpContext context);

        Task<HttpResponse> SendAsync(HttpRequest request, Func<HttpRequest, HttpContext, Task<HttpResponse>> next, HttpContext context);
    }
}