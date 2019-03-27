using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using Jal.ChainOfResponsability.Fluent.Interfaces;

namespace Jal.HttpClient.Impl
{
    public class HttpHandler : IHttpHandler
    {
        private readonly IPipelineBuilder _pipelinebuilder;

        public HttpHandler(IPipelineBuilder pipelinebuilder)
        {
            _pipelinebuilder = pipelinebuilder;
        }

        private void UpdateRequestUri(HttpRequest request)
        {
            var url = request.Message.RequestUri.AbsoluteUri;

            if (request.QueryParameters.Count > 0)
            {
                url = new UriBuilder(url) { Query = BuildQueryParameters(request.QueryParameters) }.Uri.ToString();
            }

            request.Message.RequestUri = new Uri(url);
        }

        private string BuildQueryParameters(List<HttpQueryParameter> httpparameters)
        {
            var builder = new StringBuilder();

            foreach (var httpparameter in httpparameters.Where(httpQueryParameter => !string.IsNullOrWhiteSpace(httpQueryParameter.Value)))
            {
                builder.AppendFormat("{0}={1}&", WebUtility.UrlEncode(httpparameter.Name), WebUtility.UrlEncode(httpparameter.Value));
            }

            var parameter = builder.ToString();

            if (!string.IsNullOrWhiteSpace(parameter))
            {
                parameter = parameter.Substring(0, parameter.Length - 1);
            }
            return parameter;
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request)
        {
            try
            {
                var wrapper = new HttpWrapper(request);

                var chain = _pipelinebuilder.ForAsync<HttpWrapper>();

                foreach (var type in request.MiddlewareTypes)
                {
                    chain.UseAsync(type);
                }

                UpdateRequestUri(request);

                await chain.UseAsync<HttpMiddelware>().RunAsync(wrapper);

                return wrapper.Response;
            }
            catch (Exception ex)
            {
                return new HttpResponse(request)
                {
                    Exception = ex
                };
            }
        }
    }
}