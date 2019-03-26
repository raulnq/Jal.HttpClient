using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Extensions
{
    public static class HttpContentTypeDescriptorExtensions
    {
        public static IHttpContentTypeDescriptor Utf8(this IHttpContentTypeDescriptor descriptor)
        {
            return descriptor.WithEncoding("utf-8");
        }

        public static IHttpContentTypeDescriptor Utf16(this IHttpContentTypeDescriptor descriptor)
        {
            return descriptor.WithEncoding("utf-16");
        }

        public static IHttpContentTypeDescriptor Utf7(this IHttpContentTypeDescriptor descriptor)
        {
            return descriptor.WithEncoding("utf-7");
        }

    }
}