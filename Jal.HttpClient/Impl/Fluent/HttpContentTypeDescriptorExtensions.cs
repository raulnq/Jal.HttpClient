using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Impl.Fluent
{
    public static class HttpContentTypeDescriptorExtensions
    {
        public static IHttpContentTypeDescriptor Utf8(this IHttpContentTypeDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-8");
        }

        public static IHttpContentTypeDescriptor Utf16(this IHttpContentTypeDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-16");
        }

        public static IHttpContentTypeDescriptor Utf7(this IHttpContentTypeDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-7");
        }

    }
}