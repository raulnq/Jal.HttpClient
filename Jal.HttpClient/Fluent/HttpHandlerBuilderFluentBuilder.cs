using System;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Fluent
{
    public class HttpHandlerBuilderFluentBuilder : IHttpHandlerBuilderStartFluentBuilder, IHttpHandlerBuilderEndFluentBuilder
    {
        private IHttpHandler _httpHandler;

        private IHttpHandlerBuilder _httpHandlerBuilder;

        public IHttpHandlerBuilderEndFluentBuilder UseHttpHandlerBuilder(IHttpHandlerBuilder httpHandlerBuilder)
        {
            if (httpHandlerBuilder == null)
            {
                throw new ArgumentNullException("httpHandlerBuilder");
            }
            _httpHandlerBuilder = httpHandlerBuilder;
            return this;
        }

        public IHttpHandlerBuilderEndFluentBuilder UseHttpHandler(IHttpHandler httpHandler)
        {
            if (httpHandler == null)
            {
                throw new ArgumentNullException("httpHandler");
            }
            _httpHandler = httpHandler;
            return this;
        }

        public IHttpHandlerBuilder Create
        {
            get
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
}