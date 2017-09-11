using System.IO;
using System.Net;
using System.Text;

namespace Jal.HttpClient.Model
{
    public abstract class HttpContent
    {
        public static readonly HttpContent Instance = new HttpEmptyContent();

        public string ContentType { get; set; }

        public string CharacterSet { get; set; }

        public HttpContentDisposition Disposition { get; set; }

        protected HttpContent()
        {
            ContentType = string.Empty;
            CharacterSet = string.Empty;
            Disposition = new HttpContentDisposition();
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

        public Encoding GetEncoding()
        {
            return !string.IsNullOrEmpty(CharacterSet) ? Encoding.GetEncoding(CharacterSet.Replace("charset=", "")) : GetDefaultEncoding();
        }
    }
}