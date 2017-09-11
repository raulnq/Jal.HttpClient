using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpMultiPartFormDataContentTypeDescriptor : IHttpMultiPartFormDataContentTypeDescriptor
    {
        private readonly HttpContent _httpcontent;

        public HttpMultiPartFormDataContentTypeDescriptor(HttpContent httpcontent)
        {
            _httpcontent = httpcontent;
        }

        public IHttpMultiPartFormDataContentTypeDescriptor WithCharacterSet(string characterset)
        {
            _httpcontent.CharacterSet = characterset;
            return this;
        }


        public IHttpMultiPartFormDataContentTypeDescriptor WithDisposition(string name, string filename = "")
        {
            _httpcontent.Disposition.Name = name;
            _httpcontent.Disposition.FileName = filename;
            return this;
        }

        public IHttpMultiPartFormDataContentTypeDescriptor WithContentType(string contenttype)
        {
            _httpcontent.ContentType = contenttype;
            return this;
        }
    }
}