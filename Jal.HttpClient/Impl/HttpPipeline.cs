using System;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpPipeline : IHttpPipeline
    {
        private readonly Type[] _middlewaretypes;

        private readonly IHttpMiddlewareFactory _factory;

        private int _current;

        private readonly HttpRequest _request;

        public HttpPipeline(Type[] middlewaretypes, IHttpMiddlewareFactory factory, HttpRequest request)
        {
            _middlewaretypes = middlewaretypes;
            _current = 0;
            _request = request;
            _factory = factory;
        }

        public HttpResponse Send()
        {
            return GetNext().Invoke();
        }

        private Func<HttpResponse> GetNext()
        {
            return () =>
            {
                if (_current < _middlewaretypes.Length)
                {
                    var middleware = _factory.Create(_middlewaretypes[_current]);
                    _current++;
                    return middleware.Send(_request, GetNext());
                }
                else
                {
                    var middleware = _factory.Create(_middlewaretypes[_middlewaretypes.Length-1]);
                    return middleware.Send(_request, GetNext());
                }
            };
        }

        public async Task<HttpResponse> SendAsync()
        {
            return await GetNextAsync().Invoke();
        }

        private Func<Task<HttpResponse>> GetNextAsync()
        {
            return () =>
            {
                if (_current < _middlewaretypes.Length)
                {
                    var middleware = _factory.Create(_middlewaretypes[_current]);
                    _current++;
                    return middleware.SendAsync(_request, GetNextAsync());
                }
                else
                {
                    var middleware = _factory.Create(_middlewaretypes[_middlewaretypes.Length-1]);
                    return middleware.SendAsync(_request, GetNextAsync());
                }
            };
        }
    }
}