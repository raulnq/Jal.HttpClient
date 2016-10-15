using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpRequestToWebRequestConverter : IHttpRequestToWebRequestConverter
    {
        private readonly IHttpMethodMapper _httpMethodMapper;

        private readonly Encoding _defaultEncoding;

        private readonly string _defaultContentType;

        public HttpRequestToWebRequestConverter(IHttpMethodMapper httpMethodMapper)
        {
            _httpMethodMapper = httpMethodMapper;
            _defaultEncoding = Encoding.UTF8;
            _defaultContentType = "text/plain";
        }

        public WebRequest Convert(HttpRequest httpRequest, int timeout)
        {
            var url = httpRequest.Url;

            if (httpRequest.QueryParameters.Count > 0)
            {
                url = new UriBuilder(url) { Query = BuildQueryParameters(httpRequest.QueryParameters) }.Uri.ToString();
            }

            var request = (HttpWebRequest) WebRequest.Create(new Uri(url));

            request.Method = _httpMethodMapper.Map(httpRequest.HttpMethod);

            request.Timeout = httpRequest.Timeout < 0 ? timeout : httpRequest.Timeout;

            if (!string.IsNullOrEmpty(httpRequest.AcceptedType))
            {
                request.Accept = httpRequest.AcceptedType;
            }

            request.AutomaticDecompression = httpRequest.DecompressionMethods;

            if (httpRequest.Headers.Count > 0)
            {
                WriteHeaders(httpRequest, request);
            }

            if (!string.IsNullOrWhiteSpace(httpRequest.Content))
            {
                WriteContent(httpRequest, request);
            }

            return request;

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


        private void WriteHeaders(HttpRequest httpRequest, WebRequest webRequest)
        {
            foreach (var header in httpRequest.Headers)
            {
                if (webRequest.Headers.AllKeys.Contains(header.Name))
                {
                    webRequest.Headers.Remove(header.Name);
                }
                webRequest.Headers.Add(header.Name, header.Value);
            }
        }

        private void WriteContent(HttpRequest httpRequest, WebRequest request)
        {
            Encoding encoding;

            if (!string.IsNullOrEmpty(httpRequest.ContentType))
            {
                if (!string.IsNullOrEmpty(httpRequest.CharacterSet))
                {
                    
                    request.ContentType = $"{httpRequest.ContentType}; {httpRequest.CharacterSet}";
                    encoding=Encoding.GetEncoding(httpRequest.CharacterSet.Replace("charset=", ""));
                }
                else
                {
                    request.ContentType = httpRequest.ContentType;
                    encoding = _defaultEncoding;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(httpRequest.CharacterSet))
                {
                    request.ContentType = $"text/plain; {httpRequest.CharacterSet}";
                    encoding = Encoding.GetEncoding(httpRequest.CharacterSet.Replace("charset=", ""));
                }
                else
                {
                    request.ContentType = _defaultContentType;
                    encoding = _defaultEncoding;
                }
            }

            request.ContentLength = encoding.GetByteCount(httpRequest.Content);

            using (var writeStream = request.GetRequestStream())
            {
                var bytes = encoding.GetBytes(httpRequest.Content);

                writeStream.Write(bytes, 0, bytes.Length);

                writeStream.Flush();
            }
        }
    }
}
