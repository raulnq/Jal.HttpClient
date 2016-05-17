using System;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Interface;

namespace Jal.HttpClient.Fluent
{
    public class HttpHandlerBuilderSetupDescriptor : IHttpHandlerBuilderHttpHandlerSetupDescriptor, IHttpHandlerBuilderSetupDescriptor
    {
        private IHttpHandler _httpHandler;

        private IHttpHandlerBuilder _httpHandlerBuilder;

        public IHttpHandlerBuilderSetupDescriptor UseHttpHandlerBuilder(IHttpHandlerBuilder httpHandlerBuilder)
        {
            _httpHandlerBuilder = httpHandlerBuilder;
            return this;
        }

        public IHttpHandlerBuilderSetupDescriptor UseHttpHandler(IHttpHandler httpHandler)
        {
            if (httpHandler == null)
            {
                throw new ArgumentNullException("httpHandler");
            }
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
                return new HttpHandlerBuilder(_httpHandler);
            }
        }
    }
}