using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Logger
{
    public static class HttpMiddlewareDescriptorExtensions
    {
        public static void AddCommonLogging(this IHttpMiddlewareDescriptor descriptor)
        {
            descriptor.Add<CommonLoggingMiddelware>();
        }
    }
}
