using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpQueryParameterDescriptor : IHttpQueryParameterDescriptor
    {
        private readonly HttpRequest _httpRequest;

        public HttpQueryParameterDescriptor(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public void Add(string name, string value)
        {
            _httpRequest.QueryParameters.Add(new HttpQueryParameter(name, value));
        }
    }
}