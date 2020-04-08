using System.Net.Http;

namespace Jal.HttpClient
{
    public interface IHttpMultiPartFormDataContentDescriptor
    {
        IHttpMultiPartFormDataDispositionDescriptor WithContent(HttpContent content);
    }
}