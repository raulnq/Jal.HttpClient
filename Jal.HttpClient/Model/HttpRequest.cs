using System;
using System.Collections.Generic;
using System.Net;

namespace Jal.HttpClient.Model
{
    public class HttpRequest
    {
        public string AcceptedType { get; set; }

        public DecompressionMethods DecompressionMethods { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public List<HttpHeader> Headers { get; set; }

        public List<HttpQueryParameter> QueryParameters { get; set; }

        public HttpContent Content { get; set; }
            
        public string Url { get; set; }

        public int Timeout { get; set; }

        public bool AllowWriteStreamBuffering { get; set; }

        public HttpRequest(string url, HttpMethod httpMethod)
        {
            Url = url;
            DecompressionMethods = DecompressionMethods.None;
            QueryParameters = new List<HttpQueryParameter>();
            Headers = new List<HttpHeader>();
            Content = HttpContent.Instance;
            HttpMethod = httpMethod;
            AcceptedType = string.Empty;
            Timeout = -1;
            AllowWriteStreamBuffering = true;
        }
    }
}