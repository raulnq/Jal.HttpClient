using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace Jal.HttpClient
{
    public class HttpRequest : IDisposable
    {
        public HttpRequestMessage Message { get; private set; }

        public HttpContent Content
        {
            get
            {
                return Message?.Content;
            }
            set
            {
                Message.Content = value;
            }
        }

        public System.Net.Http.HttpClient Client { get; private set; }

        public bool DisposeClient { get; }

        public List<Type> MiddlewareTypes { get; }

        public Dictionary<string,object> Context { get; }

        public List<HttpQueryParameter> QueryParameters { get; }

        public HttpTracingContext Tracing { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public HttpRequest(string uri, HttpMethod httpMethod, CancellationToken cancellationtoken = default(CancellationToken)) :
        this(uri, httpMethod, new System.Net.Http.HttpClient(), cancellationtoken)
        {
            DisposeClient = true;
        }

        public HttpRequest(string uri, HttpMethod method, Func<System.Net.Http.HttpClient> factory, CancellationToken cancellationtoken = default(CancellationToken)) :
        this(uri, method, factory(), cancellationtoken)
        {

        }

        public HttpRequest(HttpRequestMessage message, HttpRequest request)
        {
            Message = message;

            Client = request.Client;

            QueryParameters = request.QueryParameters;

            MiddlewareTypes = request.MiddlewareTypes;

            Tracing = request.Tracing;

            Context = request.Context;

            DisposeClient = request.DisposeClient;

            CancellationToken = request.CancellationToken;
        }

        public HttpRequest(string uri, HttpMethod method, System.Net.Http.HttpClient client, CancellationToken cancellationtoken=default(CancellationToken))
        {
            Message = new HttpRequestMessage(method, uri);

            Client = client;
            
            QueryParameters = new List<HttpQueryParameter>();

            MiddlewareTypes = new List<Type>();

            Tracing = new HttpTracingContext(Guid.NewGuid().ToString(), string.Empty, string.Empty);

            Context = new Dictionary<string, object>();

            DisposeClient = false;

            CancellationToken = cancellationtoken;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Message != null)
                {
                    Message.Dispose();
                }

                Message = null;

                if (Client != null && DisposeClient)
                {
                    Client.Dispose(); 
                }

                Client = null;
            }
        }
    }
}