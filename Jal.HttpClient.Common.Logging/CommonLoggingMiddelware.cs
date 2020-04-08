using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient.Common.Logging
{
    public class CommonLoggingMiddelware : IAsyncMiddleware<HttpContext>
    {
        private readonly ILog _log;

        public CommonLoggingMiddelware(ILog log)
        {
            _log = log;
        }

        private async Task<StringBuilder> BuildResponseLog(HttpResponse response, HttpRequest request)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"RequestId: {request.Tracing.RequestId}, Duration: {response.Duration} ms, {response.Message?.ToString()}");

            if (response.Exception != null)
            {
                builder.AppendLine($"{response.Exception.Message}");
            }

            if (response.Exception==null && IsString(response.Message?.Content))
            {
                builder.AppendLine($"{Truncate(await response.Message.Content.ReadAsStringAsync().ConfigureAwait(false))}");
            }

            return builder;
        }

        public bool IsString(HttpContent content)
        {
            var contenttype = content?.Headers?.ContentType?.MediaType;

            return !string.IsNullOrWhiteSpace(contenttype) && (contenttype.Contains("text") || contenttype.Contains("xml") || contenttype.Contains("json") || contenttype.Contains("html"));
        }

        private async Task<StringBuilder> BuildRequestLog(HttpRequest request)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"RequestId: {request.Tracing.RequestId}, {request.Message.ToString()}");

            if(request.Message.Content != null && request.Message.Content is StringContent)
            {
                builder.AppendLine($"{await request.Message.Content.ReadAsStringAsync().ConfigureAwait(false)}");
            }

            return builder;
        }

        public string Truncate(string content)
        {
            if (string.IsNullOrEmpty(content)) return content;
            return content.Length <= 4096 ? content : content.Substring(0, 4096) + "...";
        }

        public async Task ExecuteAsync(AsyncContext<HttpContext> context, Func<AsyncContext<HttpContext>, Task> next)
        {
            var requestbuilder = await BuildRequestLog(context.Data.Request);

            _log.Info(requestbuilder.ToString());

            await next(context);

            var responsebuilder = await BuildResponseLog(context.Data.Response, context.Data.Request);

            _log.Info(responsebuilder.ToString());
        }
    }
}
