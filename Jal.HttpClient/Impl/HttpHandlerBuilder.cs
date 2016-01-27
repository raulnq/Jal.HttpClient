using Jal.HttpClient.Fluent;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpHandlerBuilder : IHttpHandlerBuilder
    {
        private readonly IHttpHandler _httpHandler;

        public HttpHandlerBuilder(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        public HttpHandlerDescriptor Get(string url)
        {
            return new HttpHandlerDescriptor(url, _httpHandler, HttpMethod.Get);
        }

        public HttpHandlerDescriptor Post(string url)
        {
            return new HttpHandlerDescriptor(url, _httpHandler, HttpMethod.Post);
        }

        public HttpHandlerDescriptor Put(string url)
        {
            return new HttpHandlerDescriptor(url, _httpHandler, HttpMethod.Put);
        }

        public HttpHandlerDescriptor Head(string url)
        {
            return new HttpHandlerDescriptor(url, _httpHandler, HttpMethod.Head);
        }

        public HttpHandlerDescriptor Delete(string url)
        {
            return new HttpHandlerDescriptor(url, _httpHandler, HttpMethod.Delete);
        }

        public HttpHandlerDescriptor Patch(string url)
        {
            return new HttpHandlerDescriptor(url, _httpHandler, HttpMethod.Patch);
        }

        public HttpHandlerDescriptor Options(string url)
        {
            return new HttpHandlerDescriptor(url, _httpHandler, HttpMethod.Options);
        }
    }
}
