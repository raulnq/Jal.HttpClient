﻿using System.Linq;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpMiddlewareDescriptor : IHttpMiddlewareDescriptor
    {
        private readonly HttpRequest _httpRequest;

        public HttpMiddlewareDescriptor(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public void Add<THttpMiddlewareType>() where THttpMiddlewareType : IHttpMiddleware
        {
            var type = typeof(THttpMiddlewareType);
            var item = _httpRequest.MiddlewareTypes.FirstOrDefault(x => x == type);
            if (item == null)
            {
                _httpRequest.MiddlewareTypes.Add(type);
            }
        }
    }
}