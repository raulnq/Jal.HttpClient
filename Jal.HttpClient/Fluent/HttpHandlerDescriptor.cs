using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Fluent
{
    public class HttpHandlerDescriptor
    {
        private readonly HttpRequest _httpRequest;

        private readonly IHttpHandler _httpHandler;


        public HttpHandlerDescriptor(string url, IHttpHandler httpHandler, HttpMethod httpMethod)
        {
            _httpRequest = new HttpRequest(url, httpMethod);
            _httpHandler = httpHandler;

        }

        public HttpHandlerDescriptor WithDecompressionMethods(DecompressionMethods decompressionMethods)
        {
            _httpRequest.DecompressionMethods = decompressionMethods;
            return this;
        }

        public HttpHandlerDescriptor WithContentType(string contentType)
        {
            _httpRequest.ContentType = contentType;
            return this;
        }

        public HttpHandlerDescriptor WithCharacterSet(string characterSet)
        {
            _httpRequest.CharacterSet = characterSet;
            return this;
        }

        public HttpHandlerDescriptor WithTimeout(int timeout)
        {
            _httpRequest.Timeout = timeout;
            return this;
        }

        public HttpHandlerDescriptor WithContent(string content)
        {
            _httpRequest.Content = content;
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