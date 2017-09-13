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
            builder.AppendLine($"{request.HttpMethod.ToString().ToUpper()} {request.Uri.PathAndQuery} HTTP/1.1");
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

            _log.Info(builder.ToString());
        }

        public override void OnExit(HttpResponse response, HttpRequest request)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"HTTP/1.1 {response.HttpStatusCode}{response.HttpExceptionStatus}");
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

            _log.Info(builder.ToString());
        }

        public string Truncate(string content)
        {
            if (string.IsNullOrEmpty(content)) return content;
            return content.Length <= 4096 ? content : content.Substring(0, 4096) + "...";
        }
    }
}
