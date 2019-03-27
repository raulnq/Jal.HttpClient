using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Jal.HttpClient.Model
{
    public class HttpRequest : IDisposable
    {
        public HttpRequestMessage Message { get; set; }

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

        public System.Net.Http.HttpClient Client { get; internal set; }

        public bool DisposeClient { get; }

        public List<Type> MiddlewareTypes { get; }

        public Dictionary<string,object> Context { get; }

        public List<HttpQueryParameter> QueryParameters { get; }

        public HttpIdentity Identity { get; set; }

        public HttpRequest(string uri, HttpMethod httpMethod):
        this(uri, httpMethod, new System.Net.Http.HttpClient())
        {
            DisposeClient = true;
        }

        public HttpRequest(string uri, HttpMethod method, Func<System.Net.Http.HttpClient> factory) :
        this(uri, method, factory())
        {

        }

        public HttpRequest(string uri, HttpMethod method, System.Net.Http.HttpClient client)
        {
            Message = new HttpRequestMessage(method, uri);

            Client = client;
            
            QueryParameters = new List<HttpQueryParameter>();

            MiddlewareTypes = new List<Type>();

            Identity = new HttpIdentity(Guid.NewGuid().ToString());

            Context = new Dictionary<string, object>();

            DisposeClient = false;
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