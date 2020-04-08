using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace Jal.HttpClient
{
    public static class HttpDescriptorExtensions
    {
        public static IHttpDescriptor WithTracing(this IHttpDescriptor descriptor, string requestid, string parentid=null, string operationid=null)
        {
            if (string.IsNullOrWhiteSpace(requestid))
            {
                throw new ArgumentNullException(nameof(requestid));
            }
            return descriptor.WithTracing(new HttpTracingContext(requestid, parentid, operationid));
        }

        public static IHttpDescriptor MultiPartFormData(this IHttpDescriptor descriptor, Action<IHttpMultiPartFormDataContentDescriptor> contentTypeDescriptorAction)
        {
            var content = new MultipartFormDataContent();
            
            var contentdescriptor = new HttpMultiPartFormDataContentDescriptor(content);

            contentTypeDescriptorAction(contentdescriptor);

            descriptor.WithContent(content);

            return descriptor;
        }


        public static IHttpContentTypeDescriptor FormUrlEncoded(this IHttpDescriptor descriptor, KeyValuePair<string,string>[] data)
        {
            return descriptor.WithContent(new FormUrlEncodedContent(data));
        }

        public static IHttpContentTypeDescriptor Json(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("application/json");
        }

        public static IHttpContentTypeDescriptor Xml(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("text/xml");
        }

        public static IHttpContentTypeDescriptor Text(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("text/plain");

        }

        public static IHttpContentTypeDescriptor Html(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(content).WithContentType("text/html");
        }

        public static IHttpContentTypeDescriptor WithContent(this IHttpDescriptor descriptor, string content)
        {
            return descriptor.WithContent(new StringContent(content));
        }

        public static IHttpContentTypeDescriptor WithContent(this IHttpDescriptor descriptor, Stream content, int bufferlenght = 4096)
        {
            return descriptor.WithContent(new StreamContent(content, bufferlenght));
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
                        var name = property.Name;
                        var value = property.GetValue(queryParams);
                        if (value != null)
                        {
                            x.Add(name, value.ToString());
                        }
                    }
                });
            }

            return descriptor;
        }
    }
}