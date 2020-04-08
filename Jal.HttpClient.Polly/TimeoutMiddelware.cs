using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Polly;
using Polly.Timeout;

namespace Jal.HttpClient.Polly
{
    public class TimeoutMiddelware : IAsyncMiddleware<HttpContext>
    {
        public const string TIMEOUT_DURATION_IN_SECONDS_KEY = "timeoutdurationinseconds";

        public Task ExecuteAsync(AsyncContext<HttpContext> context, Func<AsyncContext<HttpContext>, Task> next)
        {
            if (context.Data.Request.Context.ContainsKey(TIMEOUT_DURATION_IN_SECONDS_KEY))
            {
                var duration = (int)context.Data.Request.Context[TIMEOUT_DURATION_IN_SECONDS_KEY];

                var policy = Policy.TimeoutAsync(duration, TimeoutStrategy.Optimistic);

                try
                {
                    return policy.ExecuteAsync(async ct =>
                    {
                        context.CancellationToken = ct;

                        await next(context);

                        return context.Data.Response;
                    }, context.CancellationToken);
                }
                catch (TimeoutRejectedException ex)
                {
                    var response = new HttpResponse(context.Data.Request, null, ex, 0);

                    context.Data.Response = response;
                }

                return next(context);
            }
            else
            {
                return next(context);
            }

        }
    }
}
