using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient
{
    public class HttpMiddelware : IAsyncMiddleware<HttpContext>
    {
        public async Task ExecuteAsync(AsyncContext<HttpContext> context, Func<AsyncContext<HttpContext>, Task> next)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            HttpResponseMessage message = null;

            Exception exception = null;

            double duration = 0;

            try
            {
                //response.Message = await request.HttpClient.SendAsync(request.Message, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                message = await context.Data.Request.Client.SendAsync(context.Data.Request.Message, context.CancellationToken).ConfigureAwait(false);
            }
            catch (WebException we)
            {
                exception = we;
            }
            catch (InvalidOperationException ioe)
            {
                exception = ioe;
            }
            catch (HttpRequestException hre)
            {
                exception = hre;
            }
            catch (TaskCanceledException tce)
            {
                exception = tce;
            }
            catch (OperationCanceledException oce)
            {
                exception = oce;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                stopWatch.Stop();

                duration = stopWatch.Elapsed.TotalMilliseconds;
            }

            context.Data.Response = new HttpResponse(context.Data.Request, message, exception, duration);
        }
    }
}