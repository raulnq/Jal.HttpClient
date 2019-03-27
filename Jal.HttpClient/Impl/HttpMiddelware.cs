using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpMiddelware : IMiddlewareAsync<HttpWrapper>
    {
        public async Task ExecuteAsync(Context<HttpWrapper> context, Func<Context<HttpWrapper>, Task> next)
        {
            context.Data.Response = await SendAsync(context.Data.Request);
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            HttpResponse response = new HttpResponse(request);

            try
            {
                //response.Message = await request.HttpClient.SendAsync(request.Message, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                response.Message = await request.HttpClient.SendAsync(request.Message).ConfigureAwait(false);

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
    }
}