using System.IO;
using System.Net;
using System.Text;

namespace Jal.HttpClient.Model
{
    public class HttpStringContent : HttpContent
    {
        protected static readonly string TextPlain = "text/plain";

        public HttpStringContent(string content)
        {
            Content = content;
        }
        public string Content { get; set; }

        public override string ToString()
        {
            return Content;
        }

        public override void Write(WebRequest request)
        {
            if (!string.IsNullOrWhiteSpace(Content))
            {
                ContentType = GetContentType();

                request.ContentLength = GetByteCount();

                using (var writeStream = request.GetRequestStream())
                {
                    WriteStream(writeStream);
                }
            }
        }

        public override long GetByteCount()
        {
            var encoding = GetEncoding();

            return encoding.GetByteCount(Content);
        }

        public override string GetDefaultContentType()
        {
            return TextPlain;
        }

        public override Encoding GetDefaultEncoding()
        {
            return Encoding.UTF8;
        }

        public override void WriteStream(Stream writeStream)
        {
            var encoding = GetEncoding();

            var bytes = encoding.GetBytes(Content);

            writeStream.Write(bytes, 0, bytes.Length);

            writeStream.Flush();
        }
    }
}