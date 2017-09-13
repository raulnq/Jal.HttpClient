using System.IO;
using System.Net;
using System.Text;

namespace Jal.HttpClient.Model
{
    public class HttpRequestEmptyContent : HttpRequestContent
    {
        public override void WriteStream(Stream writeStream)
        {

        }

        public override void Write(WebRequest request)
        {
            
        }

        public override long GetByteCount()
        {
            return 0;
        }

        public override string GetDefaultContentType()
        {
            return string.Empty;
        }

        public override Encoding GetDefaultEncoding()
        {
            return null;
        }
    }
}