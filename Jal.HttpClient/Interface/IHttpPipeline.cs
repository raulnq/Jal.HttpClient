using Jal.HttpClient.Model;
using System;
using System.Threading.Tasks;

namespace Jal.HttpClient.Interface
{
    public interface IHttpPipeline
    {
        HttpResponse Send(HttpRequest request, Type[] MiddelwareTypes);

        Task<HttpResponse> SendAsync(HttpRequest request, Type[] MiddelwareTypes);
    }
}