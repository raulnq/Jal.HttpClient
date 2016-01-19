using System.Net;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{
    public interface IHttpRequestToWebRequestConverter
    {
        WebRequest Convert(HttpRequest httpRequest, int timeout);
    }
}
