using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Common.Logging
{
    public class CommonLoggingMiddelware : IHttpMiddleware
    {
        private readonly ILog _log;

        public CommonLoggingMiddelware(ILog log)
        {
            _log = log;
        }

        public HttpResponse Send(HttpRequest request, Func<HttpRequest, HttpContext, HttpResponse> next, HttpContext context)
        {
            var builder = BuildRequestLog(request);

            _log.Info(builder.ToString());

            var response = next(request, context);

            builder = BuildResponseLog(response, request);

            _log.Info(builder.ToString());

            return response;
        }

        private StringBuilder BuildResponseLog(HttpResponse response, HttpRequest request)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"HTTP/1.1 {response.HttpStatusCode} ({request.Identity.Id})");
            builder.AppendLine($"Duration: {response.Duration} ms");
            if (response.Exception != null)
            {
                builder.AppendLine($"Exception: {response.Exception.Message}");
            }
            foreach (var httpHeader in response.Headers)
            {
                builder.AppendLine($"{httpHeader.Key}: {httpHeader.Value.FirstOrDefault()} ");
            }
            builder.AppendLine("");

            if (IsString(response.Content))
            {
                builder.AppendLine($"{Truncate(response.Content.ReadAsStringAsync().GetAwaiter().GetResult())}");
            }

            return builder;
        }

        public bool IsString(HttpContent content)
        {
            var contenttype = content.Headers.ContentType.MediaType;

            return !string.IsNullOrWhiteSpace(contenttype) && (contenttype.Contains("text") || contenttype.Contains("xml") || contenttype.Contains("json") || contenttype.Contains("html"));
        }

        private static StringBuilder BuildRequestLog(HttpRequest request)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{request.Method.ToString().ToUpper()} {request.Uri.PathAndQuery} HTTP/1.1 ({request.Identity.Id})");
            foreach (var httpHeader in request.Headers)
            {
                builder.AppendLine($"{httpHeader.Key}: {httpHeader.Value} ");
            }
            var contenttype = request.Content.Headers.ContentType.MediaType;
            if (!string.IsNullOrWhiteSpace(contenttype))
            {
                builder.AppendLine($"Content-Type: {contenttype}");
            }
            builder.AppendLine("");
            builder.AppendLine($"{request.Content}");
            return builder;
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request, Func<HttpRequest, HttpContext, Task<HttpResponse>> next, HttpContext context)
        {
            var builder = BuildRequestLog(request);

            _log.Info(builder.ToString());

            var response =  await next(request, context);

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
