using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpMiddelware : IHttpMiddleware
    {

        public HttpResponse Send(HttpRequest request, Func<HttpRequest, HttpContext, HttpResponse> next, HttpContext context)
        {
            return SendAsync(request, context).GetAwaiter().GetResult();
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request, HttpContext context)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            HttpResponse response = new HttpResponse(request);

            try
            {

                response.Message = await request.HttpClient.SendAsync(request.Message, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                return response;
                    
                
            }
            catch (WebException we)
            {
                response.Exception = we;
            }
            catch (InvalidOperationException ioe)
            {
                response.Exception = ioe;
            }
            catch (HttpRequestException hre)
            {
                response.Exception = hre;
            }
            catch (TaskCanceledException tce)
            {
                response.Exception = tce;
            }
            finally
            {
                stopWatch.Stop();

                if (response != null)
                {
                    response.Duration = stopWatch.Elapsed.TotalMilliseconds;
                }
            }
            return response;
        }

        public Task<HttpResponse> SendAsync(HttpRequest request, Func<HttpRequest, HttpContext, Task<HttpResponse>> next, HttpContext context)
        {
            return SendAsync(request, context);
        }
    }
}