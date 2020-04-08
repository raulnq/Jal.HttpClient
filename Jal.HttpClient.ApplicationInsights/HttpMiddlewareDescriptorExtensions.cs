namespace Jal.HttpClient.ApplicationInsights
{
    public static class HttpMiddlewareDescriptorExtensions
    {
        public static void UseApplicationInsights(this IHttpMiddlewareDescriptor descriptor)
        {
            descriptor.Add<ApplicationInsightsMiddelware>();
        }
    }
}
