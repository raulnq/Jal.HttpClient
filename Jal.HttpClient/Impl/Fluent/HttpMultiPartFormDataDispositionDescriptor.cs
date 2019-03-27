using Jal.HttpClient.Interface.Fluent;
using System.Net.Http;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpMultiPartFormDataDispositionDescriptor : IHttpMultiPartFormDataDispositionDescriptor, IHttpMultiPartFormDataContentTypeDescriptor
    {
        private readonly HttpContent _httpcontent;

        public HttpMultiPartFormDataDispositionDescriptor(HttpContent httpcontent)
        {
            _httpcontent = httpcontent;
        }

        public IHttpMultiPartFormDataContentTypeDescriptor WithDisposition(string name, string filename = "")
        {
            var cd = new System.Net.Http.Headers.ContentDispositionHeaderValue(name);
            if(!string.IsNullOrEmpty(filename))
            {
                cd.FileName = filename;
            }
            _httpcontent.Headers.ContentDisposition = cd;
            return this;
        }

        public IHttpMultiPartFormDataContentTypeDescriptor WithContentType(string contenttype)
        {
            _httpcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contenttype);
            return this;
        }
    }
}