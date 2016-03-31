using System.Collections.Generic;
using System.Net;

namespace Jal.HttpClient.Model
{
    public class HttpRequest
    {
        public string ContentType { get; set; }

        public string AcceptedType { get; set; }

        public string CharacterSet { get; set; }

        public DecompressionMethods DecompressionMethods { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public List<HttpHeader> Headers { get; set; }

        public List<HttpQueryParameter> QueryParameters { get; set; }
            
        public string Content { get; set; }

        public string Url { get; set; }

        public int Timeout { get; set; }

        public HttpRequest(string url, HttpMethod httpMethod)
        {
            Url = url;
            DecompressionMethods = DecompressionMethods.None;
            QueryParameters = new List<HttpQueryParameter>();
            Headers = new List<HttpHeader>();
            HttpMethod = httpMethod;
            ContentType = string.Empty;
            CharacterSet = string.Empty;
            AcceptedType = string.Empty;
            Timeout = -1;
        }
    }
}