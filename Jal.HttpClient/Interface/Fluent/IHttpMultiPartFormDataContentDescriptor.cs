using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpMultiPartFormDataContentDescriptor
    {
        IHttpMultiPartFormDataDispositionDescriptor WithContent(HttpRequestSimpleDataContent requestContent);
    }
}