using System.Collections.Generic;
using System.Net;

namespace Jal.HttpClient.Model
{
    public class HttpResponse
    {
        public string Url { get; set; }

        public string Content { get; set; }

        public byte[] Bytes { get; set; }

        public long ContentLength { get; set; }

        public string ContentType { get; set; }

        public List<HttpHeader> Headers { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public WebException WebException { get; set; }

        public double Duration { get; set; }

        public HttpResponse()
        {
            Headers=new List<HttpHeader>();
        }
    }
}