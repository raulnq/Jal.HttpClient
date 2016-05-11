using Jal.HttpClient.Impl;
using Jal.HttpClient.Interface;

namespace Jal.HttpClient.Fluent
{
    public class HttpHandlerSetupDescriptor
    {
        private IHttpRequestToWebRequestConverter _httpRequestToWebRequestConverter;

        private IWebResponseToHttpResponseConverter _responseToHttpResponseConverter;

        private IHttpInterceptor _httpInterceptor;

        private IHttpHandler _httpHandler;

        private int _timeout = 5000;

        public HttpHandlerSetupDescriptor UseHttpRequestToWebRequestConverter(IHttpRequestToWebRequestConverter converter)
        {
            _httpRequestToWebRequestConverter = converter;
            return this;
        }

        public HttpHandlerSetupDescriptor UseWebResponseToHttpResponseConverter(IWebResponseToHttpResponseConverter converter)
        {
            _responseToHttpResponseConverter = converter;
            return this;
        }

        public HttpHandlerSetupDescriptor UseHttpInterceptor(IHttpInterceptor interceptor)
        {
            _httpInterceptor = interceptor;
            return this;
        }

        public HttpHandlerSetupDescriptor UseHttpHandler(IHttpHandler httphandler)
        {
            _httpHandler = httphandler;
            return this;
        }

        public HttpHandlerSetupDescriptor WithTimeout(int timeout)
        {
            _timeout = timeout;
            return this;
        }

        public IHttpHandler Create()
        {
            if (_httpHandler == null)
            {
                IHttpRequestToWebRequestConverter requestConverter = new HttpRequestToWebRequestConverter(new HttpMethodMapper());

                IWebResponseToHttpResponseConverter responseConverter = new WebResponseToHttpResponseConverter();

                if (_httpRequestToWebRequestConverter!=null)
                {
                    requestConverter = _httpRequestToWebRequestConverter;
                    
                }
                if (_responseToHttpResponseConverter!=null)
                {
                    responseConverter = _responseToHttpResponseConverter;
                }

                var httphandler = new HttpHandler(requestConverter, responseConverter);

                if (_httpInterceptor != null)
                {
                    httphandler.HttpInterceptor = _httpInterceptor;
                }

                if (_timeout>0)
                {
                    httphandler.Timeout = _timeout;
                }

                return httphandler;
            }
            else
            {
                return _httpHandler;
            }
        }
    }
}