using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using Jal.ChainOfResponsability.Fluent.Interfaces;
using Jal.ChainOfResponsability.Intefaces;

namespace Jal.HttpClient.Impl
{
    public class HttpHandler : IHttpHandler
    {
        private readonly IPipelineBuilder _pipelinebuilder;

        public HttpHandler(IPipelineBuilder pipelinebuilder)
        {
            _pipelinebuilder = pipelinebuilder;
        }

        public HttpResponse Send(HttpRequest httpRequest)
        {
            try
            {
                var wrapper = new HttpMessageWrapper(httpRequest);

                var chain = _pipelinebuilder.For<HttpMessageWrapper>();

                foreach (var type in httpRequest.MiddlewareTypes)
                {
                    chain.Use(type);
                }

                UpdateRequestUri(httpRequest);

                chain.Use<HttpMiddelware>().Run(wrapper);

                return wrapper.Response;
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
                var wrapper = new HttpMessageWrapper(httpRequest);

                var chain = _pipelinebuilder.ForAsync<HttpMessageWrapper>();

                foreach (var type in httpRequest.MiddlewareTypes)
                {
                    chain.UseAsync(type);
                }

                UpdateRequestUri(httpRequest);

                await chain.UseAsync<HttpMiddelware>().RunAsync(wrapper);

                return wrapper.Response;
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