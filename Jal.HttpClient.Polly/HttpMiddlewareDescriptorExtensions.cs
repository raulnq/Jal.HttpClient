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

                x.Add(OnConditionRetryMiddelware.RETRY_COUNT_KEY, retrycount);
                x.Add(OnConditionRetryMiddelware.RETRY_CONDITION_KEY, retrycondition);
                if(onretry!=null)
                {
                    x.Add(OnConditionRetryMiddelware.ON_RETRY_KEY, onretry);
                }
            });
        }

        public static void UseCircuitBreaker(this IHttpMiddlewareDescriptor descriptor, AsyncCircuitBreakerPolicy<HttpResponse> policy)
        {
            descriptor.Add<CircuitBreakerMiddelware>(x => {
                x.Add(CircuitBreakerMiddelware.CIRCUIT_BREAKER_POLICY_KEY, policy);
            });
        }

        public static void UseTimeout(this IHttpMiddlewareDescriptor descriptor, int timeoutdurationinseconds=10)
        {
            descriptor.Add<TimeoutMiddelware>(x => {
                x.Add(TimeoutMiddelware.TIMEOUT_DURATION_IN_SECONDS_KEY, timeoutdurationinseconds);
            });
        }
    }
}
