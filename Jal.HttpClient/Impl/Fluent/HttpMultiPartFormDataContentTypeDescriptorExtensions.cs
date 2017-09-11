using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Impl.Fluent
{
    public static class HttpMultiPartFormDataContentTypeDescriptorExtensions
    {
        public static IHttpMultiPartFormDataContentTypeDescriptor Utf8(this IHttpMultiPartFormDataContentTypeDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-8");
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Utf16(this IHttpMultiPartFormDataContentTypeDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-16");
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Utf7(this IHttpMultiPartFormDataContentTypeDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-7");
        }
    }
}