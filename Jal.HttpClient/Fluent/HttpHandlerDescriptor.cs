using System.Linq;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Fluent
{
    public class HttpHandlerDescriptor
    {
        private readonly HttpRequest _httpRequest;

        private readonly IHttpHandler _httpHandler;


        public HttpHandlerDescriptor(string url, IHttpHandler httpHandler)
        {
            _httpRequest = new HttpRequest(url, HttpMethod.Get);
            _httpHandler = httpHandler;

        }

        public HttpHandlerDescriptor WithVerb(HttpMethod httpMethod)
        {
            _httpRequest.HttpMethod = httpMethod;
            return this;
        }

        public HttpHandlerDescriptor WithContentType(string httpContentType)
        {
            _httpRequest.HttpContentType = httpContentType;
            return this;
        }

        public HttpHandlerDescriptor WithCharacterSet(string httpCharacterSet)
        {
            _httpRequest.HttpCharacterSet = httpCharacterSet;
            return this;
        }

        public HttpHandlerDescriptor WithTimeout(int timeout)
        {
            _httpRequest.Timeout = timeout;
            return this;
        }

        public HttpHandlerDescriptor WithBody(string body)
        {
            _httpRequest.Body = body;
            return this;
        }

        public HttpHandlerDescriptor AddHeader(string name, string value)
        {
            var item = _httpRequest.Headers.FirstOrDefault(x => x.Name == name);
            if (item != null)
            {
                _httpRequest.Headers.Remove(item);
            }
            _httpRequest.Headers.Add(new HttpHeader() { Value = value, Name = name });
            return this;
        }

        public HttpHandlerDescriptor AddQueryParameter(string name, string value)
        {
            _httpRequest.QueryParameters.Add(new HttpQueryParameter() { Name = name, Value = value });
            return this;
        }

        public HttpResponse Send()
        {
            return _httpHandler.Send(_httpRequest);
        }

        public Task<HttpResponse> SendAsync()
        {
            return _httpHandler.SendAsync(_httpRequest);
        }
    }
}