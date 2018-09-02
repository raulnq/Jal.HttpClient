using System;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using Polly;

namespace Jal.HttpClient.Polly
{
    public class OnConditionRetryMiddelware : IHttpMiddleware
    {
        public HttpResponse Send(HttpRequest request, Func<HttpRequest, HttpContext, HttpResponse> next, HttpContext context)
        {
            var currentindex = context.Index;

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
                            .Retry(retrycount.Value, (c, r) => { context.Index = currentindex; onretry(c, r); })
                            .Execute(() => { return next(request, context); });
                        }
                    }

                    return Policy
                    .HandleResult<HttpResponse>(retrycondition)
                    .Retry(retrycount.Value, (c,r) => { context.Index = currentindex; })
                    .Execute(() => { return next(request, context); });
                }
            }

            return next(request, context);
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request, Func<HttpRequest, HttpContext, Task<HttpResponse>> next, HttpContext context)
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
                            .ExecuteAsync(() => { return next(request, context); });
                        }
                    }

                    return await Policy
                    .HandleResult<HttpResponse>(retrycondition)
                    .Retry(retrycount.Value)
                    .ExecuteAsync(() => { return next(request, context); });
                }
            }

            return await next(request, context);
        }
    }
}
