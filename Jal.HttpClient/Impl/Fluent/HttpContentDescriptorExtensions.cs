using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public static class HttpContentDescriptorExtensions
    {

        public static IHttpContentTypeDescriptor FormUrlEncoded(this IHttpContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(new HttpStringContent(content)).WithContentType("application/x-www-form-urlencoded").Utf8();
        }

        public static IHttpContentTypeDescriptor Json(this IHttpContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(new HttpStringContent(content)).WithContentType("application/json").Utf8();
        }

        public static IHttpContentTypeDescriptor Xml(this IHttpContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(new HttpStringContent(content)).WithContentType("text/xml").Utf8();
        }

        public static IHttpContentTypeDescriptor Text(this IHttpContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(new HttpStringContent(content)).WithContentType("text/plain").Utf8();
        }

        public static IHttpContentTypeDescriptor Html(this IHttpContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(new HttpStringContent(content)).WithContentType("text/html").Utf8();
        }

    }
}