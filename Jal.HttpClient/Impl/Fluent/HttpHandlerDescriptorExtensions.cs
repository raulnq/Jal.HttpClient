using System.Net;
using Jal.HttpClient.Interface.Fluent;

namespace Jal.HttpClient.Impl.Fluent
{
    public static class HttpHandlerDescriptorExtensions
    {
        public static IHttpHandlerDescriptor GZip(this IHttpHandlerDescriptor descriptor)
        {
            return descriptor.WithDecompressionMethods(DecompressionMethods.GZip);
        }

        public static IHttpHandlerDescriptor Deflate(this IHttpHandlerDescriptor descriptor)
        {
            return descriptor.WithDecompressionMethods(DecompressionMethods.Deflate);
        }

        public static IHttpHandlerDescriptor DeflateGZip(this IHttpHandlerDescriptor descriptor)
        {
            return descriptor.WithDecompressionMethods(DecompressionMethods.GZip | DecompressionMethods.Deflate);
        }


        public static IHttpHandlerDescriptor Utf8(this IHttpHandlerDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-8");
        }

        public static IHttpHandlerDescriptor Utf16(this IHttpHandlerDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-16");
        }

        public static IHttpHandlerDescriptor Utf7(this IHttpHandlerDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-7");
        }

        public static IHttpHandlerDescriptor FormUrlEncoded(this IHttpHandlerDescriptor descriptor, string content)
        {
            return descriptor.WithContentType("application/x-www-form-urlencoded").WithContent(content);
        }

        public static IHttpHandlerDescriptor MultipartFormData(this IHttpHandlerDescriptor descriptor, string content)
        {
            return descriptor.WithContentType("multipart/form-data").WithContent(content);
        }

        public static IHttpHandlerDescriptor Json(this IHttpHandlerDescriptor descriptor, string content)
        {
            return descriptor.WithContentType("application/json").WithContent(content);
        }

        public static IHttpHandlerDescriptor Xml(this IHttpHandlerDescriptor descriptor, string content)
        {
            return descriptor.WithContentType("text/xml").WithContent(content);
        }

        public static IHttpHandlerDescriptor WithQueryParameters(this IHttpHandlerDescriptor descriptor, object queryParams)
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