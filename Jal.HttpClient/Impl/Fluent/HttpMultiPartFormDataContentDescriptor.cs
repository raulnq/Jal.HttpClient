using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;
using System.Net.Http;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpMultiPartFormDataContentDescriptor : IHttpMultiPartFormDataContentDescriptor
    {
        private readonly MultipartFormDataContent _httpcontent;

        public HttpMultiPartFormDataContentDescriptor(MultipartFormDataContent httpcontent)
        {
            _httpcontent = httpcontent;
        }


        public IHttpMultiPartFormDataDispositionDescriptor WithContent(HttpContent requestContent)
        {
            _httpcontent.Add(requestContent);
            return new HttpMultiPartFormDataDispositionDescriptor(requestContent);
        }
    }
}