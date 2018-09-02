using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpHandler : IHttpHandler
    {
        public static IHttpHandler Current;

        private readonly IHttpMiddlewareFactory _factory;

        public static IHttpHandler Create(int timeout = 5000, IHttpMiddleware[] middlewares = null)
        {
            var requestconverter = new HttpRequestToWebRequestConverter(new HttpMethodMapper(), timeout);

            var responseconverter = new WebResponseToHttpResponseConverter();

            var all = new List<IHttpMiddleware>();

            all.Add(new HttpMiddelware(requestconverter, responseconverter));

            if(middlewares!=null)
            {
                all.AddRange(middlewares);
            }

            var httphandler = new HttpHandler(new HttpMiddlewareFactory(middlewares.ToArray()));

            return httphandler;
        }

        public HttpHandler(IHttpMiddlewareFactory factory)
        {
            _factory = factory;
        }

        public HttpResponse Send(HttpRequest httpRequest)
        {
            try
            {
                var types = new List<Type>();

                types.AddRange(httpRequest.MiddlewareTypes);

                types.Add(typeof(HttpMiddelware));

                UpdateRequestUri(httpRequest);

                var pipeline = new HttpPipeline(types.ToArray(), _factory, httpRequest);

                return pipeline.Send();
            }
            catch (Exception ex)
            {
                return new HttpResponse()
                {
                    Exception = ex
                };
            }
        }

        private void UpdateRequestUri(HttpRequest httpRequest)
        {
            var url = httpRequest.Url;

            if (httpRequest.QueryParameters.Count > 0)
            {
                url = new UriBuilder(url) { Query = BuildQueryParameters(httpRequest.QueryParameters) }.Uri.ToString();
            }

            httpRequest.Uri = new Uri(url);
        }

        private string BuildQueryParameters(List<HttpQueryParameter> httpQueryParameters)
        {
            var builder = new StringBuilder();

            foreach (var httpQueryParameter in httpQueryParameters.Where(httpQueryParameter => !string.IsNullOrWhiteSpace(httpQueryParameter.Value)))
            {
                builder.AppendFormat("{0}={1}&", WebUtility.UrlEncode(httpQueryParameter.Name), WebUtility.UrlEncode(httpQueryParameter.Value));
            }

            var parameter = builder.ToString();

            if (!string.IsNullOrWhiteSpace(parameter))
            {
                parameter = parameter.Substring(0, parameter.Length - 1);
            }
            return parameter;
        }

        public async Task<HttpResponse> SendAsync(HttpRequest httpRequest)
        {
            try
            {
                var types = new List<Type>();

                types.AddRange(httpRequest.MiddlewareTypes);

                types.Add(typeof(HttpMiddelware));

                UpdateRequestUri(httpRequest);

                var pipeline = new HttpPipeline(types.ToArray(), _factory, httpRequest);

                return await pipeline.SendAsync();
            }
            catch (Exception ex)
            {
                return new HttpResponse()
                {
                    Exception = ex
                };
            }
        }
    }
}