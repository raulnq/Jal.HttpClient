using System.Text;
using Common.Logging;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Logger
{
    public class HttpLogger : AbstractHttpInterceptor
    {
        private readonly ILog _log;

        public HttpLogger(ILog log)
        {
            _log = log;
        }

        public override void OnEntry(HttpRequest request)
        {
            var builder = new StringBuilder();
            builder.Append($"Request Url:{request.Url}");

            builder.Append($",Method:{request.HttpMethod}");
            builder.Append(",QueryParameters: ");
            foreach (var queryParameter in request.QueryParameters)
            {
                builder.Append($"{queryParameter.Name}:{queryParameter.Value} ");
            }
            builder.Append(",Headers: ");
            foreach (var httpHeader in request.Headers)
            {
                builder.Append($"{httpHeader.Name}:{httpHeader.Value} ");
            }
            builder.Append($",ContentType:{request.Content.ContentType}");
            builder.Append($",CharacterSet:{request.Content.CharacterSet}");
            builder.Append($",Content:{request.Content}");

            _log.Info(builder.ToString());
        }

        public override void OnExit(HttpResponse response, HttpRequest request)
        {
            var builder = new StringBuilder();
            builder.Append($"Response Url:{response.Url}");
            builder.Append($",Duration:{response.Duration}");
            builder.Append($",ContentType:{response.ContentType}");
            builder.Append($",ContentLength:{response.ContentLength}");
            builder.Append($",HttpStatusCode:{response.HttpStatusCode}");
            builder.Append(",Headers: ");
            foreach (var httpHeader in response.Headers)
            {
                builder.Append($"{httpHeader.Name}:{httpHeader.Value} ");
            }
            builder.Append($",Content:{response.Content}");
            if (response.WebException != null)
            {
                builder.Append($",WebException:{response.WebException.Message}");
            }
            _log.Info(builder.ToString());
        }

    }
}
