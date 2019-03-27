using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpDescriptor : IHttpDescriptor, IHttpContentTypeDescriptor
    {
        private readonly Model.HttpContextDescriptor _httpcontext;
        
        public HttpDescriptor(string url, IHttpHandler httphandler, HttpMethod httpMethod, System.Net.Http.HttpClient httpclient)
        {
            if(httpclient==null)
            {
                _httpcontext = new Model.HttpContextDescriptor(new HttpRequest(url, httpMethod), httphandler);
            }
            else
            {
                _httpcontext = new Model.HttpContextDescriptor(new HttpRequest(url, httpMethod, httpclient), httphandler);
            }
            
        }

        public IHttpDescriptor WithAcceptedType(string acceptedtype)
        {
            _httpcontext.Request.Message.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(acceptedtype));

            return this;
        }

        public IHttpContentTypeDescriptor WithContent(HttpContent requestContent)
        {
            _httpcontext.Request.Message.Content = requestContent;

            return this;
        }

        public IHttpDescriptor WithHeaders(Action<IHttpHeaderDescriptor> action)
        {
            _httpcontext.HeaderDescriptorAction = action;

            return this;
        }

        public IHttpDescriptor WithQueryParameters(Action<IHttpQueryParameterDescriptor> action)
        {
            _httpcontext.QueryParemeterDescriptorAction = action;

            return this;
        }

        public async Task<HttpResponse> SendAsync()
        {
            if (_httpcontext.QueryParemeterDescriptorAction != null)
            {
                var queryParemeterDescriptor = new HttpQueryParameterDescriptor(_httpcontext.Request);

                _httpcontext.QueryParemeterDescriptorAction(queryParemeterDescriptor);
            }

            if (_httpcontext.MiddlewareDescriptorAction != null)
            {
                var middlewareParemeterDescriptor = new HttpMiddlewareDescriptor(_httpcontext.Request);

                _httpcontext.MiddlewareDescriptorAction(middlewareParemeterDescriptor);
            }

            if (_httpcontext.HeaderDescriptorAction != null)
            {
                var headerDescriptor = new HttpHeaderDescriptor(_httpcontext.Request);

                _httpcontext.HeaderDescriptorAction(headerDescriptor);
            }

            return await _httpcontext.Handler.SendAsync(_httpcontext.Request);
        }

        public IHttpDescriptor WithMiddleware(Action<IHttpMiddlewareDescriptor> action)
        {
            _httpcontext.MiddlewareDescriptorAction = action;

            return this;
        }

        public IHttpDescriptor WithIdentity(HttpIdentity identity)
        {
            _httpcontext.Request.Identity = identity;

            return this;
        }

        public IHttpContentTypeDescriptor WithContentType(string contenttype)
        {
            _httpcontext.Request.Message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contenttype);

            return this;
        }
    }
}