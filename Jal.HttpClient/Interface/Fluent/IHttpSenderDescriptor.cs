using System.Threading.Tasks;

namespace Jal.HttpClient
{
    public interface IHttpSenderDescriptor
    {
        Task<HttpResponse> SendAsync();
    }
}