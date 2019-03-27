using System.IO;
using System.Net;
using System.Net.Http;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Extensions
{
    public static class HttpMultiPartFormDataContentDescriptorExtensions
    {
        public static IHttpMultiPartFormDataContentTypeDescriptor Json(this IHttpMultiPartFormDataContentDescriptor descriptor, string content, string name, string filename="")
        {
            return descriptor.WithContent(content).WithDisposition(name, filename).WithContentType("application/json");
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Xml(this IHttpMultiPartFormDataContentDescriptor descriptor, string content, string name, string filename = "")
        {
            return descriptor.WithContent(content).WithDisposition(name, filename).WithContentType("text/xml");
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Text(this IHttpMultiPartFormDataContentDescriptor descriptor, string content, string name, string filename = "")
        {
            return descriptor.WithContent(content).WithDisposition(name, filename).WithContentType("text/plain");
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor Html(this IHttpMultiPartFormDataContentDescriptor descriptor, string content, string name, string filename = "")
        {
            return descriptor.WithContent(content).WithDisposition(name, filename).WithContentType("text/html");
        }

        public static IHttpMultiPartFormDataContentTypeDescriptor UrlEncoded(this IHttpMultiPartFormDataContentDescriptor descriptor, string content, string name, string filename = "")
        {
            return descriptor.WithContent(WebUtility.UrlEncode(content)).WithDisposition(WebUtility.UrlEncode(name), filename);
        }

        public static IHttpMultiPartFormDataDispositionDescriptor WithContent(this IHttpMultiPartFormDataContentDescriptor descriptor, string content)
        {
            return descriptor.WithContent(new StringContent(content));
        }

        public static IHttpMultiPartFormDataDispositionDescriptor WithContent(this IHttpMultiPartFormDataContentDescriptor descriptor, Stream content, int bufferlenght = 4096)
        {
            return descriptor.WithContent(new StreamContent(content, bufferlenght));
        }
    }
}