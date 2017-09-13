using System;
using System.Collections.Generic;
using System.Net;

namespace Jal.HttpClient.Model
{
    public class HttpRequest
    {
        public string AcceptedType { get; set; }

        public DecompressionMethods Decompression { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public List<HttpHeader> Headers { get; set; }

        public List<HttpQueryParameter> QueryParameters { get; set; }

        public HttpRequestContent Content { get; set; }
            
        public string Url { get; set; } //TODO Try to Use just Uri

        public int Timeout { get; set; }

        public bool AllowWriteStreamBuffering { get; set; }

        public Uri Uri { get; set; }

        public HttpRequest(string url, HttpMethod httpMethod)
        {
            Url = url;
            Decompression = DecompressionMethods.None;
            QueryParameters = new List<HttpQueryParameter>();
            Headers = new List<HttpHeader>();
            Content = HttpRequestContent.Instance;
            HttpMethod = httpMethod;
            AcceptedType = string.Empty;
            Timeout = -1;
            AllowWriteStreamBuffering = true;
        }
    }
}