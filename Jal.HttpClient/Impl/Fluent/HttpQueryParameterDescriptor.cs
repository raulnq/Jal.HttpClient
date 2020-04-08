namespace Jal.HttpClient
{
    public class HttpQueryParameterDescriptor : IHttpQueryParameterDescriptor
    {
        private readonly HttpRequest _request;

        public HttpQueryParameterDescriptor(HttpRequest request)
        {
            _request = request;
        }

        public void Add(string name, string value)
        {
            _request.QueryParameters.Add(new HttpQueryParameter(name, value));
        }
    }
}