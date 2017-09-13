using System.IO;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public static class HttpMultiPartFormDataContentDescriptorExtensions
    {
        public static IHttpMultiPartFormDataContentTypeDescriptor Json(this IHttpMultiPartFormDataContentDescriptor descriptor, string content, string name, string filename="")
        {
            return descriptor.WithContent(content).WithDisposition(name, filename).WithContentType("application/json").Utf8();
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Xml(this IHttpMultiPartFormDataContentDescriptor descriptor, string content, string name, string filename = "")
        {
            return descriptor.WithContent(content).WithDisposition(name, filename).WithContentType("text/xml").Utf8();
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Text(this IHttpMultiPartFormDataContentDescriptor descriptor, string content, string name, string filename = "")
        {
            return descriptor.WithContent(content).WithDisposition(name, filename).WithContentType("text/plain").Utf8();
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Html(this IHttpMultiPartFormDataContentDescriptor descriptor, string content, string name, string filename = "")
        {
            return descriptor.WithContent(content).WithDisposition(name, filename).WithContentType("text/html").Utf8();
        }

        public static IHttpMultiPartFormDataDispositionDescriptor WithContent(this IHttpMultiPartFormDataContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(new HttpRequestStringContent(content));
        }

        public static IHttpMultiPartFormDataDispositionDescriptor WithContent(this IHttpMultiPartFormDataContentDescriptor descriptor, Stream content, long bufferlenght = 4096)
        {
            return descriptor.WithContent(new HttpRequestStreamContent(content, bufferlenght));
        }
    }
}