using System.IO;
using System.Net;
using System.Text;

namespace Jal.HttpClient.Model
{
    public abstract class HttpContent
    {
        public static readonly HttpContent Instance = new HttpEmptyContent();

        protected static readonly Encoding DefaultEncoding = Encoding.UTF8;

        protected static readonly string DefaultContentType = "text/plain";
        
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

        public void WriteContentType()
        {
            if (!string.IsNullOrEmpty(ContentType))
            {
                ContentType = !string.IsNullOrEmpty(CharacterSet) ? $"{ContentType}; {CharacterSet}" : ContentType;
            }
            else
            {
                ContentType = !string.IsNullOrEmpty(CharacterSet) ? $"text/plain; {CharacterSet}" : DefaultContentType;
            }
        }
    }
}