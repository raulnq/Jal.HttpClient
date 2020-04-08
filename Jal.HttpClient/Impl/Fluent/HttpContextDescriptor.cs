namespace Jal.HttpClient
{
    public class HttpContextDescriptor : IHttpContextDescriptor
    {
        private readonly HttpRequest _request;

        public HttpContextDescriptor(HttpRequest request)
        {
            _request = request;
        }

        public void Add(string name, object value)
        {
            _request.Context.Add(name, value);
        }
    }
}