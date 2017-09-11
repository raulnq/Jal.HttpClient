using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Jal.HttpClient.Impl.Fluent;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
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

        public static IHttpHandlerBuilder Builder => new HttpHandlerBuilder();

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

            try
            {
                var request = RequestConverter.Convert(httpRequest, Timeout);

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

                if (httpResponse != null)
                {
                    httpResponse.Duration = stopWatch.Elapsed.TotalMilliseconds;

                    Interceptor.OnExit(httpResponse, httpRequest);
                }
            }
        }

        public async Task<HttpResponse> SendAsync(HttpRequest httpRequest)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            Interceptor.OnEntry(httpRequest);

            HttpResponse httpResponse = null;

            try
            {
                var request = RequestConverter.Convert(httpRequest, Timeout);

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
                stopWatch.Stop();

                if (httpResponse != null)
                {
                    httpResponse.Duration = stopWatch.Elapsed.TotalMilliseconds;

                    Interceptor.OnExit(httpResponse, httpRequest);
                }
            }
        }
    }
}