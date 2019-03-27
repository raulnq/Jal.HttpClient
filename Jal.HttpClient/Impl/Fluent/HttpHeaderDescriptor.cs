using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpHeaderDescriptor : IHttpHeaderDescriptor
    {
        private readonly HttpRequest _request;

        public HttpHeaderDescriptor(HttpRequest request)
        {
            _request = request;
        }

        public void Add(string name, string value)
        {
            if (_request.Message.Headers.Contains(name))
            {
                _request.Message.Headers.Remove(name);
            }
            _request.Message.Headers.Add(name, value);
        }
    }
}