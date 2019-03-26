using Jal.HttpClient.Model;
using System.Net.Http;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMultiPartFormDataContentDescriptor
    {
        IHttpMultiPartFormDataDispositionDescriptor WithContent(HttpContent requestContent);
    }
}