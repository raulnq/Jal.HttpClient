using System.Threading.Tasks;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpSenderDescriptor
    {
        Task<HttpResponse> SendAsync();
    }
}