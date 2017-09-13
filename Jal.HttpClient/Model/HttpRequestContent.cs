using System.IO;
using System.Net;
using System.Text;

namespace Jal.HttpClient.Model
{
    public abstract class HttpRequestContent
    {
        public static readonly HttpRequestContent Instance = new HttpRequestEmptyContent();

        protected static readonly string TextPlain = "text/plain";

        protected static readonly string ApplicationOctectStream = "application/octet-stream";

        public string ContentType { get; set; }

        public string CharacterSet { get; set; }

        protected HttpRequestContent()
        {
            ContentType = string.Empty;
            CharacterSet = string.Empty;
        }

        public abstract void WriteStream(Stream writeStream);

        public abstract void Write(WebRequest request);

        public abstract long GetByteCount();

        public abstract string GetDefaultContentType();

        public abstract Encoding GetDefaultEncoding();

        public string GetContentType()
        {
            if (!string.IsNullOrEmpty(ContentType))
            {
                return !string.IsNullOrEmpty(CharacterSet) ? $"{ContentType}; {CharacterSet}" : ContentType;
            }
            else
            {
                return !string.IsNullOrEmpty(CharacterSet) ? $"{GetDefaultContentType()}; {CharacterSet}" : GetDefaultContentType();
            }
        }
    }
}