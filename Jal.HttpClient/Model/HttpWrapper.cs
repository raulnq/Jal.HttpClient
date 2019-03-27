namespace Jal.HttpClient.Model
{
    public class HttpWrapper
    {
        public HttpWrapper(HttpRequest request)
        {
            Request = request;
        }

        public HttpRequest Request { get; }

        public HttpResponse Response { get; set; }
    }
}