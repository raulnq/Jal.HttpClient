using System;
using System.Net.Http;

namespace Jal.HttpClient.Model
{
    public class HttpResponse : IDisposable
    {
        public HttpResponse(HttpRequest request)
        {
            HttpRequest = request;
        }

        public HttpRequest HttpRequest { get; internal set; }

        public HttpResponseMessage Message { get; set; }

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

                    Message = null;
                }

                if(HttpRequest!=null)
                {
                    HttpRequest.Dispose();

                    HttpRequest = null;
                }       
            }
        }
    }
}