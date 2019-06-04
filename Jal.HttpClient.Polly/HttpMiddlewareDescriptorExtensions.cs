using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;
using Polly;
using Polly.CircuitBreaker;
using System;

namespace Jal.HttpClient.Polly
{
    public static class HttpMiddlewareDescriptorExtensions
    {
        public static void OnConditionRetry(this IHttpMiddlewareDescriptor descriptor, int retrycount, Func<HttpResponse, bool>  retrycondition, Action<DelegateResult<HttpResponse>, int> onretry=null)
        {
            descriptor.Add<OnConditionRetryMiddelware>(x=> {

                x.Add("retrycount", retrycount);
                x.Add("retrycondition", retrycondition);
                if(onretry!=null)
                {
                    x.Add("onretry", onretry);
                }
            });
        }

        public static void UseCircuitBreaker(this IHttpMiddlewareDescriptor descriptor, CircuitBreakerPolicy<HttpResponse> policy)
        {
            descriptor.Add<CircuitBreakerMiddelware>(x => {
                x.Add("circuitbreakerpolicy", policy);
            });
        }
    }
}
