using System;
using System.Net.Http;

namespace Jal.HttpClient.Model
{
    public class HttpResponse : IDisposable
    {
        public HttpResponse(HttpRequest request)
        {
            Request = request;
        }

        public HttpRequest Request { get; internal set; }

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

                if(Request!=null)
                {
                    Request.Dispose();

                    Request = null;
                }       
            }
        }
    }
}