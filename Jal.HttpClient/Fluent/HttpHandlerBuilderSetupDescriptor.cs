using Jal.HttpClient.Impl;
using Jal.HttpClient.Interface;

namespace Jal.HttpClient.Fluent
{
    public class HttpHandlerBuilderSetupDescriptor
    {
        private IHttpHandler _httpHandler;

        private IHttpHandlerBuilder _httpHandlerBuilder;

        public HttpHandlerBuilderSetupDescriptor UseHttpHandlerBuilder(IHttpHandlerBuilder httpHandlerBuilder)
        {
            _httpHandlerBuilder = httpHandlerBuilder;
            return this;
        }

        public HttpHandlerBuilderSetupDescriptor UseHttpHandler(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
            return this;
        }

        public IHttpHandlerBuilder Create()
        {
            if (_httpHandlerBuilder != null)
            {
                return _httpHandlerBuilder;
            }
            else
            {
                if (_httpHandler != null)
                {
                    return new HttpHandlerBuilder(_httpHandler);
                }
                else
                {
                    var handler = new HttpHandlerSetupDescriptor().Create();
                    return new HttpHandlerBuilder(handler);
                }
            }
        }
    }
}