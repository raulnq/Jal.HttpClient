using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpMultiPartFormDataContentDescriptor : IHttpMultiPartFormDataContentDescriptor
    {
        private readonly HttpRequestMultiPartFormDataContent _httpcontent;

        public HttpMultiPartFormDataContentDescriptor(HttpRequestMultiPartFormDataContent httpcontent)
        {
            _httpcontent = httpcontent;
        }


        public IHttpMultiPartFormDataDispositionDescriptor WithContent(HttpRequestSimpleDataContent requestContent)
        {
            _httpcontent.Contents.Add(requestContent);
            return new HttpMultiPartFormDataDispositionDescriptor(requestContent);
        }
    }
}