using System;
using System.Collections.Generic;
using System.Net;

namespace Jal.HttpClient.Model
{
    public class HttpResponse : IDisposable
    {
        public Uri Uri { get; set; }

        public HttpResponseContent Content { get; set; }

        public List<HttpHeader> Headers { get; set; }

        public HttpStatusCode? HttpStatusCode { get; set; }

        public WebExceptionStatus? HttpExceptionStatus { get; set; }

        public WebException Exception { get; set; }

        public double Duration { get; set; }

        public HttpResponse()
        {
            Headers= new List<HttpHeader>();
            Content = new HttpResponseContent();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Content?.Dispose();
                Content = null;
                Headers = null;
            }
        }
    }
}