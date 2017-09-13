using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpMultiPartFormDataDispositionDescriptor : IHttpMultiPartFormDataDispositionDescriptor, IHttpMultiPartFormDataContentTypeDescriptor
    {
        private readonly HttpRequestSimpleDataContent _httpcontent;

        public HttpMultiPartFormDataDispositionDescriptor(HttpRequestSimpleDataContent httpcontent)
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