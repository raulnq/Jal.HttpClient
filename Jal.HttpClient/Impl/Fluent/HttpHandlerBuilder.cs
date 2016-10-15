using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpHandlerBuilder : IHttpHandlerBuilder
    {
        private IHttpInterceptor _httpInterceptor;

        private int _timeout = 5000;

        public IHttpHandlerBuilder UseHttpInterceptor(IHttpInterceptor interceptor)
        {
            _httpInterceptor = interceptor;
            return this;
        }


        public IHttpHandlerBuilder WithTimeout(int timeout)
        {
            _timeout = timeout;
            return this;
        }

        public IHttpHandler Create
        {
            get
            {
                IHttpRequestToWebRequestConverter requestConverter = new HttpRequestToWebRequestConverter(new HttpMethodMapper());

                IWebResponseToHttpResponseConverter responseConverter = new WebResponseToHttpResponseConverter();

                var httphandler = new HttpHandler(requestConverter, responseConverter);

                if (_httpInterceptor != null)
                {
                    httphandler.Interceptor = _httpInterceptor;
                }

                if (_timeout > 0)
                {
                    httphandler.Timeout = _timeout;
                }

                return httphandler;
            }
         }
    }
}