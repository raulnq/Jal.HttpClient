using System;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Logger
{
    public class CommonLoggingMiddelware : IHttpMiddleware
    {
        private readonly ILog _log;

        public CommonLoggingMiddelware(ILog log)
        {
            _log = log;
        }

        public HttpResponse Send(HttpRequest request, Func<HttpResponse> next)
        {
            var builder = BuildRequestLog(request);

            _log.Info(builder.ToString());

            var response = next();

            builder = BuildResponseLog(response, request);

            _log.Info(builder.ToString());

            return response;
        }

        private StringBuilder BuildResponseLog(HttpResponse response, HttpRequest request)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"HTTP/1.1 {response.HttpStatusCode}{response.HttpExceptionStatus} ({request.Identity.Id})");
            builder.AppendLine($"Duration: {response.Duration} ms");
            if (response.Exception != null)
            {
                builder.AppendLine($"Exception: {response.Exception.Message}");
            }
            foreach (var httpHeader in response.Headers)
            {
                builder.AppendLine($"{httpHeader.Name}: {httpHeader.Value} ");
            }
            builder.AppendLine("");

            if (response.Content.IsString())
            {
                builder.AppendLine($"{Truncate(response.Content.Read())}");
            }

            return builder;
        }

        private static StringBuilder BuildRequestLog(HttpRequest request)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{request.HttpMethod.ToString().ToUpper()} {request.Uri.PathAndQuery} HTTP/1.1 ({request.Identity.Id})");
            foreach (var httpHeader in request.Headers)
            {
                builder.AppendLine($"{httpHeader.Name}: {httpHeader.Value} ");
            }
            var contenttype = request.Content.GetContentType();
            if (!string.IsNullOrWhiteSpace(contenttype))
            {
                builder.AppendLine($"Content-Type: {contenttype}");
            }
            builder.AppendLine("");
            builder.AppendLine($"{request.Content}");
            return builder;
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request, Func<Task<HttpResponse>> next)
        {
            var builder = BuildRequestLog(request);

            _log.Info(builder.ToString());

            var response =  await next();

            builder = BuildResponseLog(response, request);

            _log.Info(builder.ToString());

            return response;
        }

        public string Truncate(string content)
        {
            if (string.IsNullOrEmpty(content)) return content;
            return content.Length <= 4096 ? content : content.Substring(0, 4096) + "...";
        }
    }
}
