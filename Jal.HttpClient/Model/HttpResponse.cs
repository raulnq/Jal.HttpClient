using System;
using System.Net.Http;

namespace Jal.HttpClient
{
    public class HttpResponse : IDisposable
    {
        public HttpResponse(HttpRequest request, HttpResponseMessage message, Exception exception, double duration)
        {
            Request = request;
            Message = message;
            Exception = exception;
            Duration = duration;
        }

        public HttpRequest Request { get; private set; }

        public HttpResponseMessage Message { get; private set; }

        public Exception Exception { get; }

        public double Duration { get; }

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