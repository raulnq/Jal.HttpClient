using System.Threading.Tasks;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpSenderDescriptor
    {
        HttpResponse Send { get; }

        Task<HttpResponse> SendAsync { get; }
    }
}