using System;
using System.Net.Http;

namespace Jal.HttpClient
{
    public static class HttpMiddlewareDescriptorExtensions
    {
        public static void AuthorizedByBearerToken(this IHttpMiddlewareDescriptor descriptor, string tokenvalue)
        {
            descriptor.Add<TokenAuthenticatorMiddleware>(y => { y.Add(TokenAuthenticatorMiddleware.TOKEN_VALUE_KEY, tokenvalue); y.Add(TokenAuthenticatorMiddleware.TOKEN_TYPE_KEY, "Bearer"); });
        }

        public static void AddTracing(this IHttpMiddlewareDescriptor descriptor, string requestidheadername="requestid", string parentidheadername = "parentid", string operationidheadername = "operationid")
        {
            descriptor.Add<TracingMiddleware>(y => { y.Add(TracingMiddleware.REQUESTID_HEADER_NAME_KEY, requestidheadername); y.Add(TracingMiddleware.PARENTID_HEADER_NAME_KEY, parentidheadername); y.Add(TracingMiddleware.OPERATIONID_HEADER_NAME_KEY, operationidheadername); });
        }

        public static void AuthorizedByToken(this IHttpMiddlewareDescriptor descriptor, string tokenvalue, string tokentype)
        {
            descriptor.Add<TokenAuthenticatorMiddleware>(y => { y.Add(TokenAuthenticatorMiddleware.TOKEN_VALUE_KEY, tokenvalue); y.Add(TokenAuthenticatorMiddleware.TOKEN_TYPE_KEY, tokentype); });
        }

        public static void AuthorizedByBasicHttp(this IHttpMiddlewareDescriptor descriptor, string username, string password)
        {
            descriptor.Add<BasicHttpAuthenticatorMiddleware>(y => { y.Add(BasicHttpAuthenticatorMiddleware.USER_NAME_KEY, username); y.Add(BasicHttpAuthenticatorMiddleware.PASSWORD_KEY, password); });
        }

        public static void UseMemoryCache(this IHttpMiddlewareDescriptor descriptor, double durationinseconds, Func<HttpRequest, string> keybuilder, Func<HttpResponse, bool> cachewhen, string cachemode = "sliding", Action<HttpResponseMessage> oncacheget=null)
        {
            descriptor.Add<MemoryCacheMiddleware>(y => 
            {
                y.Add(MemoryCacheMiddleware.CACHE_DURATION_IN_SECONDS_KEY, durationinseconds);
                y.Add(MemoryCacheMiddleware.CACHE_KEY_BUILDER_KEY, keybuilder);
                y.Add(MemoryCacheMiddleware.CACHE_MODE_KEY, cachemode);
                y.Add(MemoryCacheMiddleware.CACHE_WHEN_KEY, cachewhen);
                if (oncacheget!=null)
                {
                    y.Add(MemoryCacheMiddleware.ON_CACHE_GET, oncacheget);
                }
            });
        }
    }
}
