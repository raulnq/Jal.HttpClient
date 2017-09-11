using System.IO;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public static class HttpMultiPartFormDataContentDescriptorExtensions
    {
        public static IHttpMultiPartFormDataContentTypeDescriptor Json(this IHttpMultiPartFormDataContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("application/json");
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Xml(this IHttpMultiPartFormDataContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("text/xml");
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Text(this IHttpMultiPartFormDataContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("text/plain");
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Html(this IHttpMultiPartFormDataContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("text/html");
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor WithContent(this IHttpMultiPartFormDataContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(new HttpStringContent(content));
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor WithContent(this IHttpMultiPartFormDataContentDescriptor descriptor, Stream content, long bufferlenght = 4096)
        {
            return descriptor.WithContent(new HttpStreamContent(content, bufferlenght));
        }
    }
}