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

        public List<Type> MiddlewareTypes { get; set; }

        public List<HttpHeader> Headers { get; set; }

        public List<HttpQueryParameter> QueryParameters { get; set; }

        public HttpRequestContent Content { get; set; }
            
        public string Url { get; set; }

        public int Timeout { get; set; }

        public bool AllowWriteStreamBuffering { get; set; }

        public HttpIdentity Identity { get; set; }

        public Uri Uri { get; set; }

        public HttpRequest(string url, HttpMethod httpMethod)
        {
            Url = url;
            Decompression = DecompressionMethods.None;
            QueryParameters = new List<HttpQueryParameter>();
            Headers = new List<HttpHeader>();
            MiddlewareTypes = new List<Type>();
            Content = HttpRequestContent.Instance;
            HttpMethod = httpMethod;
            AcceptedType = string.Empty;
            Timeout = -1;
            AllowWriteStreamBuffering = true;
            Identity = new HttpIdentity(Guid.NewGuid().ToString());
        }
    }
}