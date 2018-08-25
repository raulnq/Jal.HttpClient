using System.Linq;
using System.Net;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpRequestToWebRequestConverter : IHttpRequestToWebRequestConverter
    {
        private readonly IHttpMethodMapper _httpMethodMapper;

        private readonly int _timeout;

        public HttpRequestToWebRequestConverter(IHttpMethodMapper httpMethodMapper, int timeout)
        {
            _httpMethodMapper = httpMethodMapper;
            _timeout = timeout;
        }

        public WebRequest Convert(HttpRequest httpRequest)
        {
            var request = (HttpWebRequest) WebRequest.Create(httpRequest.Uri);

            request.AllowWriteStreamBuffering = httpRequest.AllowWriteStreamBuffering;

            request.Method = _httpMethodMapper.Map(httpRequest.HttpMethod);

            request.Timeout = httpRequest.Timeout < 0 ? _timeout : httpRequest.Timeout;

            if (!string.IsNullOrEmpty(httpRequest.AcceptedType))
            {
                request.Accept = httpRequest.AcceptedType;
            }

            request.AutomaticDecompression = httpRequest.Decompression;

            if (httpRequest.Headers.Count > 0)
            {
                WriteHeaders(httpRequest, request);
            }

            httpRequest.Content.Write(request);
            
            return request;

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
    }
}
