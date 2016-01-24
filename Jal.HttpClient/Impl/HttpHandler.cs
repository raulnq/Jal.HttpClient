using System.Net;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpHandler : IHttpHandler
    {
        public IHttpInterceptor HttpInterceptor { get; set; }

        private readonly IHttpRequestToWebRequestConverter _httpRequestToWebRequestConverter;

        private readonly IWebResponseToHttpResponseConverter _webResponseToHttpResponseConverter;

        public int Timeout { get; set; }

        public HttpHandler(IHttpRequestToWebRequestConverter httpRequestToWebRequestConverter, IWebResponseToHttpResponseConverter webResponseToHttpResponseConverter)
        {
            HttpInterceptor = AbstractHttpInterceptor.Instance;

            _httpRequestToWebRequestConverter = httpRequestToWebRequestConverter;

            _webResponseToHttpResponseConverter = webResponseToHttpResponseConverter;
        }

        public HttpResponse Send(HttpRequest httpRequest)
        {
            HttpInterceptor.OnEntry(httpRequest);

            HttpResponse httpResponse = null;

            var request = _httpRequestToWebRequestConverter.Convert(httpRequest, Timeout);

            try
            {
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    httpResponse = _webResponseToHttpResponseConverter.Convert(response);

                    HttpInterceptor.OnSuccess(httpResponse, httpRequest);

                    return httpResponse;
                }
            }
            catch (WebException wex)
            {
                httpResponse = _webResponseToHttpResponseConverter.Convert(wex);

                HttpInterceptor.OnError(httpResponse, httpRequest, wex);

                return httpResponse;
            }
            finally
            {
                HttpInterceptor.OnExit(httpResponse, httpRequest);
            }
        }

        public async Task<HttpResponse> SendAsync(HttpRequest httpRequest)
        {
            HttpInterceptor.OnEntry(httpRequest);

            HttpResponse httpResponse = null;

            var request = _httpRequestToWebRequestConverter.Convert(httpRequest, Timeout);

            try
            {
               using (var response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    httpResponse = _webResponseToHttpResponseConverter.Convert(response);

                    HttpInterceptor.OnSuccess(httpResponse, httpRequest);

                    return httpResponse;
                }
            }
            catch (WebException wex)
            {
                httpResponse = _webResponseToHttpResponseConverter.Convert(wex);

                HttpInterceptor.OnError(httpResponse, httpRequest, wex);

                return httpResponse;
            }
            finally
            {
                HttpInterceptor.OnExit(httpResponse, httpRequest);
            }
        }
    }
}