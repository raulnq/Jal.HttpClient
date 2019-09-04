using System;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.HttpClient.Model;
using System.Linq;
namespace Jal.HttpClient.Serilog
{
    public class SerilogMiddelware : IMiddlewareAsync<HttpWrapper>
    {
        private async Task BuildResponseLog(HttpResponse response, HttpRequest request)
        {
            var log = Log.Logger;

            if (response.Message.Content != null && IsString(response.Message.Content))
            {
                try
                {
                    log = log.ForContext("content", await response.Message.Content.ReadAsStringAsync().ConfigureAwait(false), true);
                }
                catch (Exception)
                {
                    log = log.ForContext("content", "error to log", true);
                }
                
            }

            if (response.Message != null && response.Message.Headers != null)
            {
                try
                {
                    log = log.ForContext("headers", response.Message.Headers.ToDictionary(x => x.Key, x => x.Value), true);
                }
                catch (Exception)
                {
                    log = log.ForContext("headers", "error to log", true);
                }
                
            }

            if (response.Message!=null)
            {
                if (response.Exception != null)
                {
                    log.Error(response.Exception, "Id: {id}, Duration: {duration} ms, StatusCode: {statuscode}, ReasonPhrase: {reasonphrase}, Version: {version}", request.Identity.Id, response.Duration, response.Message.StatusCode, response.Message.ReasonPhrase.ToString(), response.Message.Version.ToString());
                }
                else
                {
                    log.Information("Id: {id}, Duration: {duration} ms, StatusCode: {statuscode}, ReasonPhrase: {reasonphrase}, Version: {version}", request.Identity.Id, response.Duration, response.Message.StatusCode, response.Message.ReasonPhrase.ToString(), response.Message.Version.ToString());
                }
            }
            else
            {
                if (response.Exception != null)
                {
                    log.Error(response.Exception, "Id: {id}, Duration: {duration} ms", request.Identity.Id, response.Duration);
                }
                else
                {
                    log.Information("Id: {id}, Duration: {duration} ms", request.Identity.Id, response.Duration);
                }
                
            }
        }

        public bool IsString(HttpContent content)
        {
            var contenttype = content?.Headers?.ContentType?.MediaType;

            return !string.IsNullOrWhiteSpace(contenttype) && (contenttype.Contains("text") || contenttype.Contains("xml") || contenttype.Contains("json") || contenttype.Contains("html"));
        }

        private async Task BuildRequestLog(HttpRequest request)
        {
            var log = Log.Logger;

            if (request.Message.Content != null && request.Message.Content is StringContent)
            {
                try
                {
                    log = log.ForContext("content", await request.Message.Content.ReadAsStringAsync().ConfigureAwait(false), true);
                }
                catch (Exception)
                {
                    log = log.ForContext("content", "error to log", true);
                }
                
            }

            if(request.Message.Headers!=null)
            {
                try
                {
                    log = log.ForContext("headers", request.Message.Headers.ToDictionary(x => x.Key, x => x.Value), true);
                }
                catch (Exception)
                {
                    log = log.ForContext("headers", "error to log", true);
                }
                
            }

            log.Information("Id: {id}, Method: {method}, RequestUri: {requesturi}, Version: {version}", request.Identity.Id, request.Message.Method, request.Message.RequestUri.ToString(), request.Message.Version.ToString());
        }

        public async Task ExecuteAsync(Context<HttpWrapper> context, Func<Context<HttpWrapper>, Task> next)
        {
            await BuildRequestLog(context.Data.Request);

            await next(context);

            await BuildResponseLog(context.Data.Response, context.Data.Request);
        }
    }
}
