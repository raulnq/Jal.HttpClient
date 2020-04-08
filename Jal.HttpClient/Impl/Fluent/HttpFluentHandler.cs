using System;
using System.Net.Http;
using System.Threading;

namespace Jal.HttpClient
{
    public class HttpFluentHandler : IHttpFluentHandler
    {
        private readonly IHttpHandler _handler;

        public HttpFluentHandler(IHttpHandler handler)
        {
            _handler = handler;
        }

        public IHttpDescriptor Get(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken))
        {
            if(factory!=null)
            {
                return new HttpDescriptor(url, _handler, HttpMethod.Get, factory, cancellationtoken);
            }
            return new HttpDescriptor(url, _handler, HttpMethod.Get, httpclient, cancellationtoken);
        }

        public IHttpDescriptor Post(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken))
        {
            if (factory != null)
            {
                return new HttpDescriptor(url, _handler, HttpMethod.Post, factory, cancellationtoken);
            }
            return new HttpDescriptor(url, _handler, HttpMethod.Post, httpclient, cancellationtoken);
        }

        public IHttpDescriptor Put(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken))
        {
            if (factory != null)
            {
                return new HttpDescriptor(url, _handler, HttpMethod.Put, factory, cancellationtoken);
            }
            return new HttpDescriptor(url, _handler, HttpMethod.Put, httpclient, cancellationtoken);
        }

        public IHttpDescriptor Head(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken))
        {
            if (factory != null)
            {
                return new HttpDescriptor(url, _handler, HttpMethod.Head, factory, cancellationtoken);
            }
            return new HttpDescriptor(url, _handler, HttpMethod.Head, httpclient, cancellationtoken);
        }

        public IHttpDescriptor Delete(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken))
        {
            if (factory != null)
            {
                return new HttpDescriptor(url, _handler, HttpMethod.Delete, factory, cancellationtoken);
            }
            return new HttpDescriptor(url, _handler, HttpMethod.Delete, httpclient, cancellationtoken);
        }

        public IHttpDescriptor Patch(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken))
        {
            if (factory != null)
            {
                return new HttpDescriptor(url, _handler, new HttpMethod("PATCH"), factory, cancellationtoken);
            }
            return new HttpDescriptor(url, _handler, new HttpMethod("PATCH"), httpclient, cancellationtoken);
        }

        public IHttpDescriptor Options(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken))
        {
            if (factory != null)
            {
                return new HttpDescriptor(url, _handler, HttpMethod.Options, factory, cancellationtoken);
            }
            return new HttpDescriptor(url, _handler, HttpMethod.Options, httpclient, cancellationtoken);
        }
    }


}
