using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using System.Net.Http;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpFluentHandler : IHttpFluentHandler
    {
        private readonly IHttpHandler _handler;

        public HttpFluentHandler(IHttpHandler handler)
        {
            _handler = handler;
        }

        public IHttpDescriptor Get(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, _handler, HttpMethod.Get, httpclient);
        }

        public IHttpDescriptor Post(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, _handler, HttpMethod.Post, httpclient);
        }

        public IHttpDescriptor Put(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, _handler, HttpMethod.Put, httpclient);
        }

        public IHttpDescriptor Head(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, _handler, HttpMethod.Head, httpclient);
        }

        public IHttpDescriptor Delete(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, _handler, HttpMethod.Delete, httpclient);
        }

        public IHttpDescriptor Patch(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, _handler, new HttpMethod("PATCH"), httpclient);
        }

        public IHttpDescriptor Options(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, _handler, HttpMethod.Options, httpclient);
        }
    }


}
