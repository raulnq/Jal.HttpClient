using System.IO;
using System.Net;
using System.Text;

namespace Jal.HttpClient.Model
{
    public class HttpStringContent : HttpContent
    {
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
                WriteContentType();

                using (var writeStream = request.GetRequestStream())
                {
                    WriteStream(writeStream);
                }
            }
        }

        public override void WriteStream(Stream writeStream)
        {
            var encoding = !string.IsNullOrEmpty(CharacterSet) ? Encoding.GetEncoding(CharacterSet.Replace("charset=", "")) : DefaultEncoding;

            var bytes = encoding.GetBytes(Content);

            writeStream.Write(bytes, 0, bytes.Length);

            writeStream.Flush();
        }
    }
}