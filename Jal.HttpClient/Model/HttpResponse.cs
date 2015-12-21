using System.Collections.Generic;
using System.Net;

namespace Jal.HttpClient.Model
{
    public class HttpResponse
    {
        public string Content { get; set; }

        public string ErrorMessage { get; set; }

        public WebException ErrorException { get; set; }

        public string Url { get; set; }

        public string ContentType { get; set; }

        public List<HttpHeader> Headers { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public string StatusDescription { get; set; }
        
        public WebExceptionStatus WebExceptionStatus { get; set; }

        public long ContentLength { get; set; }

        public HttpResponse()
        {
            Headers=new List<HttpHeader>();
        }


    }
}