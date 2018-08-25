using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpDataDescriptor : IHttpDataDescriptor
    {
        private readonly HttpRequest _httpRequest;

        public HttpDataDescriptor(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public void Add(string name, string value)
        {
            _httpRequest.Data.Add(name, value);
        }
    }
}