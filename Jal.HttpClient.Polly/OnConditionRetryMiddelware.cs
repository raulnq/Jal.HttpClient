using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.HttpClient.Extensions;
using Jal.HttpClient.Model;
using Polly;

namespace Jal.HttpClient.Polly
{
    public class OnConditionRetryMiddelware : IMiddlewareAsync<HttpWrapper>
    {
        public async Task ExecuteAsync(Context<HttpWrapper> context, Func<Context<HttpWrapper>, Task> next)
        {
            var currentindex = context.Index;

            if (context.Data.Request.Context.ContainsKey("retrycount") && context.Data.Request.Context.ContainsKey("retrycondition"))
            {
                var retrycount = context.Data.Request.Context["retrycount"] as int?;

                var retrycondition = context.Data.Request.Context["retrycondition"] as Func<HttpResponse, bool>;

                if (retrycount != null && retrycondition != null)
                {
                    if (context.Data.Request.Context.ContainsKey("onretry"))
                    {
                        if (context.Data.Request.Context["onretry"] is Action<DelegateResult<HttpResponse>, int> onretry)
                        {
                            await Policy
                            .HandleResult<HttpResponse>(retrycondition)
                            .RetryAsync(retrycount.Value, (c, r) => { context.Index = currentindex; onretry(c, r); context.Data.Request.Message = context.Data.Request.Message.Clone(); })
                            .ExecuteAsync(async () =>
                            {

                                await next(context);

                                return context.Data.Response;
                            });
                        }
                    }
                    else
                    {
                        await Policy
                        .HandleResult<HttpResponse>(retrycondition)
                        .RetryAsync(retrycount.Value, (c, r) => { context.Index = currentindex; context.Data.Request.Message = context.Data.Request.Message.Clone(); })
                               .ExecuteAsync(async () => {

                                   await next(context);

                                   return context.Data.Response;
                               });
                    }
                }
                else
                {
                    await next(context);
                }
            }
            else
            {
                await next(context);
            }
            
        }
    }
}
