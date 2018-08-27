using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Impl.Fluent
{
    public static class HttpMiddlewareDescriptorExtensions
    {
        public static void AuthorizedByBearerToken(this IHttpMiddlewareDescriptor descriptor, string tokenvalue)
        {
            descriptor.Add<TokenAuthenticatorHttpMiddleware>(y => { y.Add("tokenvalue", tokenvalue); y.Add("tokentype", "Bearer"); });
        }

        public static void AuthorizedByToken(this IHttpMiddlewareDescriptor descriptor, string tokenvalue, string tokentype)
        {
            descriptor.Add<TokenAuthenticatorHttpMiddleware>(y => { y.Add("tokenvalue", tokenvalue); y.Add("tokentype", tokentype); });
        }

        public static void AuthorizedByBasicHttp(this IHttpMiddlewareDescriptor descriptor, string username, string password)
        {
            descriptor.Add<BasicHttpAuthenticatorHttpMiddleware>(y => { y.Add("username", username); y.Add("password", password); });
        }
    }
}
