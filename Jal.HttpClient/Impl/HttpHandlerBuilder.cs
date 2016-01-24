using System;
using System.Collections.Generic;
using System.Text;
using Jal.HttpClient.Fluent;
using Jal.HttpClient.Interface;

namespace Jal.HttpClient.Impl
{
    public class HttpHandlerBuilder : IHttpHandlerBuilder
    {
        private readonly IHttpHandler _httpHandler;

        public HttpHandlerBuilder(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        public HttpHandlerDescriptor For(string url)
        {
            return new HttpHandlerDescriptor(url, _httpHandler);
        }
    }
}
