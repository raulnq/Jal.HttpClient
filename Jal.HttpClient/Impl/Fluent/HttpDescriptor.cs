using System;
using System.Net;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpDescriptor : IHttpDescriptor
    {
        private readonly HttpDescriptorContext _httpcontext;
        
        public HttpDescriptor(string url, IHttpHandler httpHandler, HttpMethod httpMethod)
        {
            _httpcontext = new HttpDescriptorContext(new HttpRequest(url, httpMethod), httpHandler);
        }

        public IHttpDescriptor WithDecompression(DecompressionMethods decompression)
        {
            _httpcontext.HttpRequest.Decompression = decompression;
            return this;
        }



        public IHttpDescriptor WithAllowWriteStreamBuffering(bool allowwritestreambuffering)
        {
            _httpcontext.HttpRequest.AllowWriteStreamBuffering = allowwritestreambuffering;
            return this;
        }

        public IHttpDescriptor WithAcceptedType(string acceptedtype)
        {
            _httpcontext.HttpRequest.AcceptedType = acceptedtype;
            return this;
        }

        public IHttpContentTypeDescriptor WithContent(HttpRequestContent requestContent)
        {
            _httpcontext.HttpRequest.Content = requestContent;
            return new HttpContentDescriptor(_httpcontext.HttpRequest.Content, _httpcontext);
        }

        public IHttpDescriptor WithTimeout(int timeout)
        {
            _httpcontext.HttpRequest.Timeout = timeout;
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

        public HttpResponse Send
        {
            get
            {
                if (_httpcontext.QueryParemeterDescriptorAction != null)
                {
                    var queryParemeterDescriptor = new HttpQueryParameterDescriptor(_httpcontext.HttpRequest);
                    _httpcontext.QueryParemeterDescriptorAction(queryParemeterDescriptor);
                }

                if (_httpcontext.HeaderDescriptorAction != null)
                {
                    var headerDescriptor = new HttpHeaderDescriptor(_httpcontext.HttpRequest);
                    _httpcontext.HeaderDescriptorAction(headerDescriptor);
                }

                return _httpcontext.HttpHandler.Send(_httpcontext.HttpRequest);
            }

        }

        public Task<HttpResponse> SendAsync
        {
            get
            {
                if (_httpcontext.QueryParemeterDescriptorAction != null)
                {
                    var queryParemeterDescriptor = new HttpQueryParameterDescriptor(_httpcontext.HttpRequest);
                    _httpcontext.QueryParemeterDescriptorAction(queryParemeterDescriptor);
                }

                if (_httpcontext.HeaderDescriptorAction != null)
                {
                    var headerDescriptor = new HttpHeaderDescriptor(_httpcontext.HttpRequest);
                    _httpcontext.HeaderDescriptorAction(headerDescriptor);
                }

                return _httpcontext.HttpHandler.SendAsync(_httpcontext.HttpRequest);
            }
        }

    }
}