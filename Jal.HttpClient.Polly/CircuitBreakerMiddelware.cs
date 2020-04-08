using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Polly.CircuitBreaker;

namespace Jal.HttpClient.Polly
{
    public class CircuitBreakerMiddelware : IAsyncMiddleware<HttpContext>
    {
        public const string CIRCUIT_BREAKER_POLICY_KEY = "circuitbreakerpolicy";

        public Task ExecuteAsync(AsyncContext<HttpContext> context, Func<AsyncContext<HttpContext>, Task> next)
        {
            if (context.Data.Request.Context.ContainsKey(CIRCUIT_BREAKER_POLICY_KEY))
            {
                var policy = context.Data.Request.Context[CIRCUIT_BREAKER_POLICY_KEY] as AsyncCircuitBreakerPolicy<HttpResponse>;

                try
                {
                    return policy.ExecuteAsync(async () =>
                    {
                        await next(context);

                        return context.Data.Response;
                    });
                }
                catch (BrokenCircuitException<HttpResponse> ex)
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
