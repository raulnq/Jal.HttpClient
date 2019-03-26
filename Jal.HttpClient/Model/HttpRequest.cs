using System;
using System.Collections.Generic;
using System.Net;

namespace Jal.HttpClient.Model
{

    public class HttpRequest : IDisposable
    {
        public System.Net.Http.HttpRequestMessage Message { get; }

        public System.Net.Http.HttpContent Content
        {
            get
            {
                return Message.Content;
            }
            set
            {
                Message.Content = value;
            }
        }

        public System.Net.Http.HttpMethod Method
        {
            get
            {
                return Message.Method;
            }
        }

        public System.Net.Http.Headers.HttpRequestHeaders Headers
        {
            get
            {
                return Message.Headers;
            }
        }

        public Uri Uri
        {
            get
            {
                return Message.RequestUri;
            }
            set
            {
                Message.RequestUri = value;
            }
        }

        public System.Net.Http.HttpClient HttpClient { get; set; }

        public List<Type> MiddlewareTypes { get; set; }

        public Dictionary<string,object> Context { get; set; }

        public List<HttpQueryParameter> QueryParameters { get; set; }

        public int Timeout {
            get
            {
                return HttpClient.Timeout.Milliseconds;
            }
            set
            {
                if(value>0)

                HttpClient.Timeout = TimeSpan.FromMilliseconds(value);
            }

        }

        public HttpIdentity Identity { get; set; }

        public HttpRequest(string uri, System.Net.Http.HttpMethod httpMethod):
        this(uri, httpMethod, new System.Net.Http.HttpClient())
        {

        }

        public HttpRequest(string uri, System.Net.Http.HttpMethod httpMethod, Func<System.Net.Http.HttpClient> factory) :
        this(uri, httpMethod, factory())
        {

        }

        public HttpRequest(string uri, System.Net.Http.HttpMethod httpMethod, System.Net.Http.HttpClient httpclient)
        {
            Message = new System.Net.Http.HttpRequestMessage(httpMethod, uri);

            HttpClient = httpclient;
            
            QueryParameters = new List<HttpQueryParameter>();

            MiddlewareTypes = new List<Type>();

            Identity = new HttpIdentity(Guid.NewGuid().ToString());

            Context = new Dictionary<string, object>();
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
                if (HttpClient != null)
                {
                    HttpClient.Dispose();
                }
            }
        }
    }
}