using System.Net;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpHandler : IHttpHandler
    {
        public IHttpLogger HttpLogger { get; set; }

        private readonly IHttpRequestToWebRequestConverter _httpRequestToWebRequestConverter;

        private readonly IWebResponseToHttpResponseConverter _webResponseToHttpResponseConverter;

        public int Timeout { get; set; }

        public HttpHandler(IHttpRequestToWebRequestConverter httpRequestToWebRequestConverter, IWebResponseToHttpResponseConverter webResponseToHttpResponseConverter)
        {
            HttpLogger = NullHttpLogger.Instance;

            _httpRequestToWebRequestConverter = httpRequestToWebRequestConverter;

            _webResponseToHttpResponseConverter = webResponseToHttpResponseConverter;
        }

        public HttpResponse Send(HttpRequest httpRequest)
        {
            HttpLogger.Log(httpRequest);

            var request = _httpRequestToWebRequestConverter.Convert(httpRequest, Timeout);

            try
            {
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    var httpResponse = _webResponseToHttpResponseConverter.Convert(response);

                    HttpLogger.Log(httpResponse);

                    return httpResponse;
                }
            }
            catch (WebException wex)
            {
                var httpResponse = _webResponseToHttpResponseConverter.Convert(wex);

                HttpLogger.Log(httpResponse);

                return httpResponse;
            }
        }

        public async Task<HttpResponse> SendAsync(HttpRequest httpRequest)
        {
            HttpLogger.Log(httpRequest);

            var request = _httpRequestToWebRequestConverter.Convert(httpRequest, Timeout);

            try
            {
               using (var response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    var httpResponse = _webResponseToHttpResponseConverter.Convert(response);

                    HttpLogger.Log(httpResponse);

                    return httpResponse;
                }
            }
            catch (WebException wex)
            {
                var httpResponse = _webResponseToHttpResponseConverter.Convert(wex);

                HttpLogger.Log(httpResponse);

                return httpResponse;
            }
        }
    }
}