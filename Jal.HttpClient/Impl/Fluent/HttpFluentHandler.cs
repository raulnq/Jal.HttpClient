using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;
using System.Net.Http;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpFluentHandler : IHttpFluentHandler
    {
        public IHttpHandler Handler { get; }

        public HttpFluentHandler(IHttpHandler httpHandler)
        {
            Handler = httpHandler;
        }

        public IHttpDescriptor Get(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Get, httpclient);
        }

        public IHttpDescriptor Post(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Post, httpclient);
        }

        public IHttpDescriptor Put(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Put, httpclient);
        }

        public IHttpDescriptor Head(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Head, httpclient);
        }

        public IHttpDescriptor Delete(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Delete, httpclient = null);
        }

        public IHttpDescriptor Patch(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, Handler, new HttpMethod("PATCH"), httpclient);
        }

        public IHttpDescriptor Options(string url, System.Net.Http.HttpClient httpclient = null)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Options, httpclient);
        }
    }


}
