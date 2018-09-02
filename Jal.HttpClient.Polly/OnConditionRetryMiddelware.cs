using System;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using Polly;

namespace Jal.HttpClient.Polly
{
    public class OnConditionRetryMiddelware : IHttpMiddleware
    {
        public HttpResponse Send(HttpRequest request, Func<HttpResponse> next)
        {
            if(request.Context.ContainsKey("retrycount") && request.Context.ContainsKey("retrycondition"))
            {
                var retrycount = request.Context["retrycount"] as int?;

                var retrycondition = request.Context["retrycondition"] as Func<HttpResponse, bool>;

                if (retrycount != null && retrycondition != null)
                {

                    if(request.Context.ContainsKey("onretry"))
                    {
                        var onretry = request.Context["onretry"] as Action<DelegateResult<HttpResponse>, int>;

                        if (onretry != null)
                        {
                            return Policy
                            .HandleResult<HttpResponse>(retrycondition)
                            .Retry(retrycount.Value, onretry)
                            .Execute(() => { return next(); });
                        }
                    }

                    return Policy
                    .HandleResult<HttpResponse>(retrycondition)
                    .Retry(retrycount.Value)
                    .Execute(() => { return next(); });
                }
            }

            return next();
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request, Func<Task<HttpResponse>> next)
        {
            if (request.Context.ContainsKey("retrycount") && request.Context.ContainsKey("retrycondition"))
            {
                var retrycount = request.Context["retrycount"] as int?;

                var retrycondition = request.Context["retrycondition"] as Func<HttpResponse, bool>;

                if (retrycount != null && retrycondition != null)
                {
                    if (request.Context.ContainsKey("onretry"))
                    {
                        var onretry = request.Context["onretry"] as Action<DelegateResult<HttpResponse>, int>;

                        if (onretry != null)
                        {
                            return await Policy
                            .HandleResult<HttpResponse>(retrycondition)
                            .Retry(retrycount.Value, onretry)
                            .ExecuteAsync(() => { return next(); });
                        }
                    }

                    return await Policy
                    .HandleResult<HttpResponse>(retrycondition)
                    .Retry(retrycount.Value)
                    .ExecuteAsync(() => { return next(); });
                }
            }

            return await next();
        }
    }
}
