using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Jal.HttpClient.Model
{
    public class HttpResponse : IDisposable
    {


        public HttpResponse(HttpRequest request)
        {
            HttpRequest = request;
        }

        public HttpRequest HttpRequest { get; }

        public HttpResponseMessage Message { get; set; }

        public HttpContent Content
        {
            get
            {
                return Message?.Content;
            }
        }

        public HttpResponseHeaders Headers
        {
            get
            {
                return Message?.Headers;
            }
        }

        public HttpStatusCode? HttpStatusCode
        {
            get
            {
                return Message?.StatusCode;
            }
        }

        public Exception Exception { get; set; }

        public double Duration { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(Message!=null)
                {
                    Message.Dispose();
                }

                if(HttpRequest!=null)
                {
                    HttpRequest.Dispose();
                }       
            }
        }
    }
}