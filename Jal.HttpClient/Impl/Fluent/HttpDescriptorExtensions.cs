using System;
using System.IO;
using System.Net;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public static class HttpDescriptorExtensions
    {
        public static IHttpDescriptor GZip(this IHttpDescriptor descriptor)
        {
            return descriptor.WithDecompressionMethods(DecompressionMethods.GZip);
        }

        public static IHttpDescriptor Deflate(this IHttpDescriptor descriptor)
        {
            return descriptor.WithDecompressionMethods(DecompressionMethods.Deflate);
        }

        public static IHttpDescriptor DeflateGZip(this IHttpDescriptor descriptor)
        {
            return descriptor.WithDecompressionMethods(DecompressionMethods.GZip | DecompressionMethods.Deflate);
        }

        public static IHttpDescriptor MultiPartFormData(this IHttpDescriptor descriptor, Action<IHttpMultiPartFormDataContentDescriptor> contentTypeDescriptorAction)
        {
            var content = new HttpMultiPartFormDataContent();

            var contentdescriptor = new HttpMultiPartFormDataContentDescriptor(content);

            contentTypeDescriptorAction(contentdescriptor);

            descriptor.WithContent(content).WithContentType("multipart/form-data").Utf8();

            return descriptor;
        }


        public static IHttpContentTypeDescriptor FormUrlEncoded(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("application/x-www-form-urlencoded").Utf8();
        }

        public static IHttpContentTypeDescriptor Json(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("application/json").Utf8();
        }

        public static IHttpContentTypeDescriptor Xml(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("text/xml").Utf8();
        }

        public static IHttpContentTypeDescriptor Text(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("text/plain").Utf8();

        }

        public static IHttpContentTypeDescriptor Html(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("text/html").Utf8();
        }

        public static IHttpContentTypeDescriptor WithContent(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(new HttpStringContent(content));
        }

        public static IHttpContentTypeDescriptor WithContent(this IHttpDescriptor descriptor, Stream content, long bufferlenght = 4096)
        {
            return descriptor.WithContent(new HttpStreamContent(content, bufferlenght));
        }

        public static IHttpDescriptor WithQueryParameters(this IHttpDescriptor descriptor, object queryParams)
        {
            if (queryParams != null)
            {     
                var properties = queryParams.GetType().GetProperties();

                descriptor.WithQueryParameters(x =>
                {
                    foreach (var property in properties)
                    {
                        var key = property.Name;
                        var value = property.GetValue(queryParams);
                        if (value != null)
                        {
                            x.Add(key, value.ToString());
                        }
                    }
                });
            }

            return descriptor;
        }
    }
}