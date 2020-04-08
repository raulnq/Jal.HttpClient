using System.Collections.Generic;
using System.Net.Http;

namespace Jal.HttpClient
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage Clone(this HttpRequestMessage message)
        {
            HttpRequestMessage clone = new HttpRequestMessage(message.Method, message.RequestUri)
            {
                Content = message.Content,
                Version = message.Version
            };

            foreach (KeyValuePair<string, object> prop in message.Properties)
            {
                clone.Properties.Add(prop);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in message.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}