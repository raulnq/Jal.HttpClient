namespace Jal.HttpClient.Model
{
    public class HttpMessageWrapper
    {
        public HttpMessageWrapper(HttpRequest request)
        {
            Request = request;
        }

        public HttpRequest Request { get; }

        public HttpResponse Response { get; set; }
    }
}