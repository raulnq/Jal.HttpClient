using System.Collections.Generic;
using System.Linq;

namespace Jal.HttpClient.Model
{
    public class HttpRequest
    {
        public string HttpContentType { get; set; }

        public string HttpCharacterSet { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public List<HttpHeader> Headers { get; set; }

        public List<HttpQueryParameter> QueryParameters { get; set; }
            
        public string Body { get; set; }

        public string Url { get; set; }

        public int Timeout { get; set; }

        public HttpRequest(string url, HttpMethod httpMethod)
        {
            Url = url;
            QueryParameters = new List<HttpQueryParameter>();
            Headers = new List<HttpHeader>();
            HttpMethod = httpMethod;
            HttpContentType = string.Empty;
            HttpCharacterSet = "charset=UTF-8";
            Timeout = -1;
        }
    }
}