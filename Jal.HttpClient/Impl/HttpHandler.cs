using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient
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
                var context = new HttpContext(request);

                var chain = _pipelinebuilder.ForAsync<HttpContext>();

                foreach (var type in request.MiddlewareTypes)
                {
                    chain.UseAsync(type);
                }

                UpdateRequestUri(request);

                await chain.UseAsync<HttpMiddelware>().RunAsync(context, request.CancellationToken);

                return context.Response;
            }
            catch (Exception ex)
            {
                return new HttpResponse(request, null, ex, 0);
            }
        }
    }
}