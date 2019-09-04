using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Serilog
{
    public static class HttpMiddlewareDescriptorExtensions
    {
        public static void UseSerilog(this IHttpMiddlewareDescriptor descriptor)
        {
            descriptor.Add<SerilogMiddelware>();
        }
    }
}
