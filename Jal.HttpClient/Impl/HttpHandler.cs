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

        private readonly IHttpPipeline _pipeline;

        //public static IHttpHandler Create(int timeout = 5000, IHttpMiddleware[] middlewares = null)
        //{
        //    var requestconverter = new HttpRequestToWebRequestConverter(new HttpMethodMapper(), timeout);

        //    var responseconverter = new WebResponseToHttpResponseConverter();

        //    var all = new List<IHttpMiddleware>();

        //    all.Add(new HttpMiddelware(requestconverter, responseconverter));

        //    if(middlewares!=null)
        //    {
        //        all.AddRange(middlewares);
        //    }

        //    var httphandler = new HttpHandler(new HttpPipeline(new HttpMiddlewareFactory(middlewares.ToArray())));

        //    return httphandler;
        //}

        public HttpHandler(IHttpPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public HttpResponse Send(HttpRequest httpRequest)
        {
            try
            {
                var types = new List<Type>();

                types.AddRange(httpRequest.MiddlewareTypes);

                types.Add(typeof(HttpMiddelware));

                UpdateRequestUri(httpRequest);

                return _pipeline.Send(httpRequest, types.ToArray());
            }
            catch (Exception ex)
            {
                return new HttpResponse(httpRequest)
                {
                    Exception = ex
                };
            }
        }

        private void UpdateRequestUri(HttpRequest httpRequest)
        {
            var url = httpRequest.Message.RequestUri.AbsoluteUri;

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

                return await _pipeline.SendAsync(httpRequest, types.ToArray());
            }
            catch (Exception ex)
            {
                return new HttpResponse(httpRequest)
                {
                    Exception = ex
                };
            }
        }
    }
}