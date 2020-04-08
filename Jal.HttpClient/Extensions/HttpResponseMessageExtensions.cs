using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jal.HttpClient
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<HttpResponseMessage> Clone(this HttpResponseMessage message)
        {
            var clone = new HttpResponseMessage(message.StatusCode);
            var ms = new MemoryStream();

            foreach (var v in message.Headers) clone.Headers.TryAddWithoutValidation(v.Key, v.Value);
            if (message.Content != null)
            {
                await message.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                foreach (var v in message.Content.Headers) clone.Content.Headers.TryAddWithoutValidation(v.Key, v.Value);

            }
            return clone;
        }
    }
}