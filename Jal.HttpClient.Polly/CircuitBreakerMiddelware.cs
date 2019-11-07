using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.HttpClient.Model;
using Polly.CircuitBreaker;

namespace Jal.HttpClient.Polly
{
    public class CircuitBreakerMiddelware : IMiddlewareAsync<HttpWrapper>
    {
        public Task ExecuteAsync(Context<HttpWrapper> context, Func<Context<HttpWrapper>, Task> next)
        {
            if (context.Data.Request.Context.ContainsKey("circuitbreakerpolicy"))
            {
                var policy = context.Data.Request.Context["circuitbreakerpolicy"] as CircuitBreakerPolicy<HttpResponse>;

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
                    var response = new HttpResponse(context.Data.Request)
                    {
                        Exception = ex
                    };

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
