using System.Net;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Fluent
{
    public static class HttpHandlerDescriptorExtensions
    {
        public static HttpHandlerDescriptor GZip(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithDecompressionMethods(DecompressionMethods.GZip);
        }

        public static HttpHandlerDescriptor Deflate(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithDecompressionMethods(DecompressionMethods.Deflate);
        }

        public static HttpHandlerDescriptor DeflateGZip(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithDecompressionMethods(DecompressionMethods.GZip | DecompressionMethods.Deflate);
        }


        public static HttpHandlerDescriptor Utf8(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-8");
        }

        public static HttpHandlerDescriptor Utf16(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-16");
        }

        public static HttpHandlerDescriptor Utf7(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithCharacterSet("charset=UTF-7");
        }

        public static HttpHandlerDescriptor FormUrlEncoded(this HttpHandlerDescriptor descriptor, string content)
        {
            return descriptor.WithContentType("application/x-www-form-urlencoded").WithContent(content);
        }

        public static HttpHandlerDescriptor MultipartFormData(this HttpHandlerDescriptor descriptor, string content)
        {
            return descriptor.WithContentType("multipart/form-data").WithContent(content);
        }

        public static HttpHandlerDescriptor Json(this HttpHandlerDescriptor descriptor, string content)
        {
            return descriptor.WithContentType("application/json").WithContent(content);
        }

        public static HttpHandlerDescriptor Xml(this HttpHandlerDescriptor descriptor, string content)
        {
            return descriptor.WithContentType("text/xml").WithContent(content);
        }

        public static HttpHandlerDescriptor QueryParams(this HttpHandlerDescriptor descriptor, object queryParams)
        {
            if (queryParams != null)
            {     
                var properties = queryParams.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var key = property.Name;
                    var value = property.GetValue(queryParams);

                    descriptor.AddQueryParameter(key, value.ToString());
                }
            }

            return descriptor;
        }
    }
}