using Jal.HttpClient.Model;
using System.Threading.Tasks;

namespace Jal.HttpClient.Interface
{
    public interface IHttpPipeline
    {
        HttpResponse Send();

        Task<HttpResponse> SendAsync();
    }
}