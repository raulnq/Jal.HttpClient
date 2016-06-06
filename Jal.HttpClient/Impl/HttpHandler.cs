using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Jal.HttpClient.Fluent;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpHandler : IHttpHandler
    {
        public IHttpInterceptor Interceptor { get; set; }

        public IHttpRequestToWebRequestConverter RequestConverter { get; set; }

        public IWebResponseToHttpResponseConverter ResponseConverter { get; set; }

        public int Timeout { get; set; }

        public static IHttpHandler Current;

        public static HttpHandlerFluentBuilder Builder
        {
            get
            {
                return new HttpHandlerFluentBuilder();
            } 
        }

        public HttpHandler(IHttpRequestToWebRequestConverter httpRequestToWebRequestConverter, IWebResponseToHttpResponseConverter webResponseToHttpResponseConverter)
        {
            Interceptor = AbstractHttpInterceptor.Instance;

            RequestConverter = httpRequestToWebRequestConverter;

            ResponseConverter = webResponseToHttpResponseConverter;
        }

        public HttpResponse Send(HttpRequest httpRequest)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Interceptor.OnEntry(httpRequest);

            HttpResponse httpResponse = null;

            var request = RequestConverter.Convert(httpRequest, Timeout);

            try
            {
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    httpResponse = ResponseConverter.Convert(response);

                    Interceptor.OnSuccess(httpResponse, httpRequest);

                    return httpResponse;
                }
            }
            catch (WebException wex)
            {
                httpResponse = ResponseConverter.Convert(wex);

                Interceptor.OnError(httpResponse, httpRequest, wex);

                return httpResponse;
            }
            finally
            {
                stopWatch.Stop();

                httpResponse.Duration = stopWatch.Elapsed.TotalMilliseconds;

                Interceptor.OnExit(httpResponse, httpRequest);
            }
        }

        public async Task<HttpResponse> SendAsync(HttpRequest httpRequest)
        {
            Interceptor.OnEntry(httpRequest);

            HttpResponse httpResponse = null;

            var request = RequestConverter.Convert(httpRequest, Timeout);

            try
            {
               using (var response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    httpResponse = ResponseConverter.Convert(response);

                    Interceptor.OnSuccess(httpResponse, httpRequest);

                    return httpResponse;
                }
            }
            catch (WebException wex)
            {
                httpResponse = ResponseConverter.Convert(wex);

                Interceptor.OnError(httpResponse, httpRequest, wex);

                return httpResponse;
            }
            finally
            {
                Interceptor.OnExit(httpResponse, httpRequest);
            }
        }
    }
}