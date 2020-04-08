namespace Jal.HttpClient
{
    public class HttpContext
    {
        public HttpContext(HttpRequest request)
        {
            Request = request;
        }

        public HttpRequest Request { get; set; }

        public HttpResponse Response { get; set; }
    }
}