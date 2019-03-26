using System.Threading.Tasks;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpSenderDescriptor
    {
        HttpResponse Send();

        Task<HttpResponse> SendAsync();
    }
}