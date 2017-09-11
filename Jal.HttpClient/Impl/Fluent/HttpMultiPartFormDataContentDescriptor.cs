using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpMultiPartFormDataContentDescriptor : IHttpMultiPartFormDataContentDescriptor
    {
        private readonly HttpMultiPartFormDataContent _httpcontent;

        public HttpMultiPartFormDataContentDescriptor(HttpMultiPartFormDataContent httpcontent)
        {
            _httpcontent = httpcontent;
        }


        public IHttpMultiPartFormDataContentTypeDescriptor WithContent(HttpContent content)
        {
            _httpcontent.Contents.Add(content);
            return new HttpMultiPartFormDataContentTypeDescriptor(content);
        }
    }
}