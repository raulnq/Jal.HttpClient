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
            builder.Append(string.Format("Request Url:{0}", request.Url));
            builder.Append(string.Format(",ContentType:{0}", request.ContentType));
            builder.Append(string.Format(",CharacterSet:{0}", request.CharacterSet));
            builder.Append(string.Format(",Method:{0}", request.HttpMethod));
            builder.Append(",QueryParameters: ");
            foreach (var queryParameter in request.QueryParameters)
            {
                builder.Append(string.Format("{0}:{1} ", queryParameter.Name, queryParameter.Value));
            }
            builder.Append(",Headers: ");
            foreach (var httpHeader in request.Headers)
            {
                builder.Append(string.Format("{0}:{1} ", httpHeader.Name, httpHeader.Value));
            }
            builder.Append(string.Format(",Content:{0}", request.Content));
            _log.Info(builder.ToString());
        }

        public override void OnExit(HttpResponse response, HttpRequest request)
        {
            var builder = new StringBuilder();
            builder.Append(string.Format("Response Url:{0}", response.Url));
            builder.Append(string.Format(",Duration:{0}", response.Duration));
            builder.Append(string.Format(",ContentType:{0}", response.ContentType));
            builder.Append(string.Format(",ContentLength:{0}", response.ContentLength));
            builder.Append(string.Format(",HttpStatusCode:{0}", response.HttpStatusCode));
            builder.Append(",Headers: ");
            foreach (var httpHeader in response.Headers)
            {
                builder.Append(string.Format("{0}:{1} ", httpHeader.Name, httpHeader.Value));
            }
            builder.Append(string.Format(",Content:{0}", response.Content));
            if (response.WebException != null)
            {
                builder.Append(string.Format(",WebException:{0}", response.WebException.Message));
            }
            _log.Info(builder.ToString());
        }

    }
}
