using Jal.HttpClient.Model;

namespace Jal.HttpClient.Fluent
{
    public static class HttpHandlerDescriptorExtensions
    {
        public static HttpHandlerDescriptor Get(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithVerb(HttpMethod.Get);
        }

        public static HttpHandlerDescriptor Delete(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithVerb(HttpMethod.Delete);
        }

        public static HttpHandlerDescriptor FormUrlEncoded(this HttpHandlerDescriptor descriptor, string body)
        {
            return descriptor.WithContentType("application/x-www-form-urlencoded").WithBody(body);
        }

        public static HttpHandlerDescriptor MultipartFormData(this HttpHandlerDescriptor descriptor, string body)
        {
            return descriptor.WithContentType("multipart/form-data").WithBody(body);
        }

        public static HttpHandlerDescriptor Json(this HttpHandlerDescriptor descriptor, string body)
        {
            return descriptor.WithContentType("application/json").WithBody(body);
        }

        public static HttpHandlerDescriptor Xml(this HttpHandlerDescriptor descriptor, string body)
        {
            return descriptor.WithVerb(HttpMethod.Post).WithContentType("text/xml").WithBody(body);
        }

        public static HttpHandlerDescriptor Put(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithVerb(HttpMethod.Put);
        }

        public static HttpHandlerDescriptor Post(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithVerb(HttpMethod.Post);
        }

        public static HttpHandlerDescriptor Patch(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithVerb(HttpMethod.Patch);
        }

        public static HttpHandlerDescriptor Head(this HttpHandlerDescriptor descriptor)
        {
            return descriptor.WithVerb(HttpMethod.Head).WithContentType("text/xml");
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