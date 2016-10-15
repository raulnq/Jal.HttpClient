using System;
using System.Net;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpHandlerDescriptor : IHttpHandlerDescriptor
    {
        private readonly HttpRequest _httpRequest;

        private readonly IHttpHandler _httpHandler;

        private Action<IQueryParameterDescriptor> _queryParemeterDescriptorAction;

        private Action<IHeaderDescriptor> _headerDescriptorAction;

        public HttpHandlerDescriptor(string url, IHttpHandler httpHandler, HttpMethod httpMethod)
        {
            _httpRequest = new HttpRequest(url, httpMethod);
            _httpHandler = httpHandler;

        }

        public IHttpHandlerDescriptor WithDecompressionMethods(DecompressionMethods decompressionMethods)
        {
            _httpRequest.DecompressionMethods = decompressionMethods;
            return this;
        }

        public IHttpHandlerDescriptor WithContentType(string contentType)
        {
            _httpRequest.ContentType = contentType;
            return this;
        }

        public IHttpHandlerDescriptor WithAcceptedType(string acceptedType)
        {
            _httpRequest.AcceptedType = acceptedType;
            return this;
        }

        public IHttpHandlerDescriptor WithCharacterSet(string characterSet)
        {
            _httpRequest.CharacterSet = characterSet;
            return this;
        }

        public IHttpHandlerDescriptor WithTimeout(int timeout)
        {
            _httpRequest.Timeout = timeout;
            return this;
        }

        public IHttpHandlerDescriptor WithContent(string content)
        {
            _httpRequest.Content = content;
            return this;
        }

        public IHttpHandlerDescriptor WithHeaders(Action<IHeaderDescriptor> headerDescriptorAction)
        {
            _headerDescriptorAction = headerDescriptorAction;
            return this;
        }

        public IHttpHandlerDescriptor WithQueryParameters(Action<IQueryParameterDescriptor> queryParemeterDescriptorAction)
        {
            _queryParemeterDescriptorAction = queryParemeterDescriptorAction;

            return this;
        }

        public HttpResponse Send
        {
            get
            {
                if (_queryParemeterDescriptorAction != null)
                {
                    var queryParemeterDescriptor = new QueryParameterDescriptor(_httpRequest);
                    _queryParemeterDescriptorAction(queryParemeterDescriptor);
                }

                if (_headerDescriptorAction != null)
                {
                    var headerDescriptor = new HeaderDescriptor(_httpRequest);
                    _headerDescriptorAction(headerDescriptor);
                }

                return _httpHandler.Send(_httpRequest);
            }
           
        }

        public Task<HttpResponse> SendAsync
        {
            get
            {
                if (_queryParemeterDescriptorAction != null)
                {
                    var queryParemeterDescriptor = new QueryParameterDescriptor(_httpRequest);
                    _queryParemeterDescriptorAction(queryParemeterDescriptor);
                }

                if (_headerDescriptorAction != null)
                {
                    var headerDescriptor = new HeaderDescriptor(_httpRequest);
                    _headerDescriptorAction(headerDescriptor);
                }

                return _httpHandler.SendAsync(_httpRequest);
            }
        }
    }
}