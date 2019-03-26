using System.Linq;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpHeaderDescriptor : IHttpHeaderDescriptor
    {
        private readonly HttpRequest _httpRequest;

        public HttpHeaderDescriptor(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public void Add(string name, string value)
        {
            if (_httpRequest.Headers.Contains(name))
            {
                _httpRequest.Headers.Remove(name);
            }
            _httpRequest.Headers.Add(name, value);
        }
    }
}