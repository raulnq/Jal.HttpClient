using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Polly;

namespace Jal.HttpClient.Polly
{
    public class OnConditionRetryMiddelware : IAsyncMiddleware<HttpContext>
    {
        public const string RETRY_COUNT_KEY = "retrycount";

        public const string RETRY_CONDITION_KEY = "retrycondition";

        public const string ON_RETRY_KEY = "onretry";

        public Task ExecuteAsync(AsyncContext<HttpContext> context, Func<AsyncContext<HttpContext>, Task> next)
        {
            var currentindex = context.Index;

            if (context.Data.Request.Context.ContainsKey(RETRY_COUNT_KEY) && context.Data.Request.Context.ContainsKey(RETRY_CONDITION_KEY))
            {
                var retrycount = context.Data.Request.Context[RETRY_COUNT_KEY] as int?;

                var retrycondition = context.Data.Request.Context[RETRY_CONDITION_KEY] as Func<HttpResponse, bool>;

                if (retrycount != null && retrycondition != null)
                {
                    if (context.Data.Request.Context.ContainsKey(ON_RETRY_KEY) && context.Data.Request.Context[ON_RETRY_KEY] is Action<DelegateResult<HttpResponse>, int> onretry)
                    {
                        return Policy
                        .HandleResult<HttpResponse>(retrycondition)
                        .RetryAsync(retrycount.Value, (c, r) => { context.Index = currentindex; onretry(c, r); context.Data.Request = new HttpRequest(context.Data.Request.Message.Clone(), context.Data.Request); })
                        .ExecuteAsync(async () =>
                        {
                            await next(context);

                            return context.Data.Response;
                        });
                    }
                    else
                    {
                        return Policy
                        .HandleResult<HttpResponse>(retrycondition)
                        .RetryAsync(retrycount.Value, (c, r) => { context.Index = currentindex; context.Data.Request = new HttpRequest(context.Data.Request.Message.Clone(), context.Data.Request); })
                               .ExecuteAsync(async () => {

                                   await next(context);

                                   return context.Data.Response;
                               });
                    }
                }
                else
                {
                    return next(context);
                }
            }
            else
            {
                return next(context);
            }
            
        }
    }
}
