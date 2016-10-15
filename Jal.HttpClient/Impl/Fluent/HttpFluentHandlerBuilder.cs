using System;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpFluentHandlerBuilder : IHttpFluentHandlerStartBuilder, IHttpFluentHandlerEndBuilder
    {
        private IHttpHandler _httpHandler;

        public IHttpFluentHandlerEndBuilder UseHttpHandler(IHttpHandler httpHandler)
        {
            if (httpHandler == null)
            {
                throw new ArgumentNullException(nameof(httpHandler));
            }
            _httpHandler = httpHandler;
            return this;
        }

        public IHttpFluentHandler Create => new HttpFluentHandler(_httpHandler);
    }
}