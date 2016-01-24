using System.Net;
using System.Text;
using Common.Logging;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Interface;
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
            builder.Append(string.Format(",ContentType:{0}", request.HttpContentType));
            builder.Append(string.Format(",CharacterSet:{0}", request.HttpCharacterSet));
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
            builder.Append(string.Format(",Body:{0}", request.Body));
            _log.Info(builder.ToString());
        }

        public override void OnExit(HttpResponse response, HttpRequest request)
        {
            var builder = new StringBuilder();
            builder.Append(string.Format("Response Url:{0}", response.Url));
            builder.Append(string.Format(",ContentType:{0}", response.ContentType));
            builder.Append(string.Format(",ContentLength:{0}", response.ContentLength));
            builder.Append(string.Format(",HttpStatusCode:{0}", response.HttpStatusCode));
            builder.Append(string.Format(",StatusDescription:{0}", response.StatusDescription));
            builder.Append(",Headers: ");
            foreach (var httpHeader in response.Headers)
            {
                builder.Append(string.Format("{0}:{1} ", httpHeader.Name, httpHeader.Value));
            }
            builder.Append(string.Format(",Content:{0}", response.Content));
            builder.Append(string.Format(",ErrorMessage:{0}", response.ErrorMessage));
            _log.Info(builder.ToString());
        }

    }
}
