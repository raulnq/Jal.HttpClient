using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.ApplicationInsights
{
    public static class HttpMiddlewareDescriptorExtensions
    {
        public static void AddApplicationInsight(this IHttpMiddlewareDescriptor descriptor)
        {
            descriptor.Add<ApplicationInsightsMiddelware>();
        }
    }
}
