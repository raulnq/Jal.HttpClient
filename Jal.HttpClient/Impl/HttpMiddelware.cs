using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{

    public class HttpMiddelware : IHttpMiddleware
    {
        private readonly IHttpRequestToWebRequestConverter _requestconverter;

        private readonly IWebResponseToHttpResponseConverter _responseconverter;

        public HttpMiddelware(IHttpRequestToWebRequestConverter requestconverter, IWebResponseToHttpResponseConverter responseconverter)
        {
            _requestconverter = requestconverter;

            _responseconverter = responseconverter;
        }

        public HttpResponse Send(HttpRequest httprequest, Func<HttpResponse> next)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            HttpResponse httpresponse = null;

            try
            {
                var request = _requestconverter.Convert(httprequest);

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    httpresponse = _responseconverter.Convert(response);

                    return httpresponse;
                } 
            }
            catch (WebException wex)
            {
                httpresponse = _responseconverter.Convert(wex);

                return httpresponse;
            }
            finally
            {
                stopWatch.Stop();

                if (httpresponse != null)
                {
                    httpresponse.Duration = stopWatch.Elapsed.TotalMilliseconds;
                }
            }
        }

        public async Task<HttpResponse> SendAsync(HttpRequest httprequest, Func<Task<HttpResponse>> next)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            HttpResponse httpresponse = null;

            try
            {
                var request = _requestconverter.Convert(httprequest);

                using (var response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    httpresponse = _responseconverter.Convert(response);

                    return httpresponse;
                }
            }
            catch (WebException wex)
            {
                httpresponse = _responseconverter.Convert(wex);

                return httpresponse;
            }
            finally
            {
                stopWatch.Stop();

                if (httpresponse != null)
                {
                    httpresponse.Duration = stopWatch.Elapsed.TotalMilliseconds;
                }
            }
        }
    }
}