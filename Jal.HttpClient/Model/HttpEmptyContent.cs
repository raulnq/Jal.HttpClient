using System.IO;
using System.Net;

namespace Jal.HttpClient.Model
{
    public class HttpEmptyContent : HttpContent
    {
        public override void WriteStream(Stream writeStream)
        {

        }

        public override void Write(WebRequest request)
        {
            
        }
    }
}