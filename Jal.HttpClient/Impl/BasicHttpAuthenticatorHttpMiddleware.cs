using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class BasicHttpAuthenticatorHttpMiddleware : IHttpMiddleware
    {
        public HttpResponse Send(HttpRequest request, Func<HttpResponse> next)
        {
            var item = request.Headers.FirstOrDefault(x => x.Name == "Authorization");
            if (item != null)
            {
                request.Headers.Remove(item);
            }

            var user = request.Data["username"];

            var password = request.Data["password"];

            request.Headers.Add(new HttpHeader("Authorization", $"{"Basic"} {Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + password))}"));

            return next();
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request, Func<Task<HttpResponse>> next)
        {
            var item = request.Headers.FirstOrDefault(x => x.Name == "Authorization");
            if (item != null)
            {
                request.Headers.Remove(item);
            }

            var user = request.Data["username"];

            var password = request.Data["password"];

            request.Headers.Add(new HttpHeader("Authorization", $"{"Basic"} {Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + password))}"));

            return await next();
        }
    }
}
