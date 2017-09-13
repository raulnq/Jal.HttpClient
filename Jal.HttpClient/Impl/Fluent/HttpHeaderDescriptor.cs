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
            var item = _httpRequest.Headers.FirstOrDefault(x => x.Name == name);
            if (item != null)
            {
                _httpRequest.Headers.Remove(item);
            }
            _httpRequest.Headers.Add(new HttpHeader(name, value));
        }
    }
}