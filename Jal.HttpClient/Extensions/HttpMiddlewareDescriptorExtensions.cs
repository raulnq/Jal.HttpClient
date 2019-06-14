using Jal.HttpClient.Impl;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;
using System;
using System.Net.Http;

namespace Jal.HttpClient.Extensions
{
    public static class HttpMiddlewareDescriptorExtensions
    {
        public static void AuthorizedByBearerToken(this IHttpMiddlewareDescriptor descriptor, string tokenvalue)
        {
            descriptor.Add<TokenAuthenticatorMiddleware>(y => { y.Add("tokenvalue", tokenvalue); y.Add("tokentype", "Bearer"); });
        }

        public static void AddTrackingInformation(this IHttpMiddlewareDescriptor descriptor, string idheadername="correlationid", string parentidheadername = "parentid", string operationidheadername = "operationid")
        {
            descriptor.Add<IdentityTrackerMiddleware>(y => { y.Add("idheadername", idheadername); y.Add("parentidheadername", parentidheadername); y.Add("operationidheadername", operationidheadername); });
        }

        public static void AuthorizedByToken(this IHttpMiddlewareDescriptor descriptor, string tokenvalue, string tokentype)
        {
            descriptor.Add<TokenAuthenticatorMiddleware>(y => { y.Add("tokenvalue", tokenvalue); y.Add("tokentype", tokentype); });
        }

        public static void AuthorizedByBasicHttp(this IHttpMiddlewareDescriptor descriptor, string username, string password)
        {
            descriptor.Add<BasicHttpAuthenticatorMiddleware>(y => { y.Add("username", username); y.Add("password", password); });
        }

        public static void UseMemoryCache(this IHttpMiddlewareDescriptor descriptor, double durationinseconds, Func<HttpRequest, string> keybuilder, Func<HttpResponse, bool> cachewhen, string cachemode = "sliding", Action<HttpResponseMessage> oncacheget=null)
        {
            descriptor.Add<MemoryCacheMiddleware>(y => 
            {
                y.Add("durationinseconds", durationinseconds);
                y.Add("keybuilder", keybuilder);
                y.Add("cachemode", cachemode);
                y.Add("cachewhen", cachewhen);
                if (oncacheget!=null)
                {
                    y.Add("oncacheget", oncacheget);
                }
            });
        }
    }
}
