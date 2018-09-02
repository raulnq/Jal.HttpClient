using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpContextDescriptor : IHttpContextDescriptor
    {
        private readonly HttpRequest _httpRequest;

        public HttpContextDescriptor(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public void Add(string name, object value)
        {
            _httpRequest.Context.Add(name, value);
        }
    }
}