using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Jal.HttpClient.Impl.Fluent;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Extensions
{
    public static class HttpDescriptorExtensions
    {
        public static IHttpDescriptor WithIdentity(this IHttpDescriptor descriptor, string id, string parentid=null, string operationid=null)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            return descriptor.WithIdentity(new HttpIdentity(id) { ParentId = parentid, OperationId = operationid });
        }

        public static IHttpDescriptor MultiPartFormData(this IHttpDescriptor descriptor, Action<IHttpMultiPartFormDataContentDescriptor> contentTypeDescriptorAction)
        {
            var content = new MultipartFormDataContent();

            var contentdescriptor = new HttpMultiPartFormDataContentDescriptor(content);

            contentTypeDescriptorAction(contentdescriptor);

            descriptor.WithContent(content)/*.WithContentType("multipart/form-data")*/;

            return descriptor;
        }


        public static IHttpContentTypeDescriptor FormUrlEncoded(this IHttpDescriptor descriptor, KeyValuePair<string,string>[] data)
        {
            var content = string.Empty;

            var first = true;

            foreach (var keyvalue in data)
            {
                if (first)
                {
                    content = content + $"{WebUtility.UrlEncode(keyvalue.Key)}={WebUtility.UrlEncode(keyvalue.Value)}";
                    first = false;
                }
                else
                {
                    content = content + $"&{WebUtility.UrlEncode(keyvalue.Key)}={WebUtility.UrlEncode(keyvalue.Value)}";
                }
            }

            return descriptor.WithContent(content).WithContentType("application/x-www-form-urlencoded");
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