namespace Jal.HttpClient.Common.Logging
{
    public static class HttpMiddlewareDescriptorExtensions
    {
        public static void UseCommonLogging(this IHttpMiddlewareDescriptor descriptor)
        {
            descriptor.Add<CommonLoggingMiddelware>();
        }
    }
}
