using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{

    public class BasicHttpAuthenticatorMiddleware : IHttpMiddleware
    {
        public HttpResponse Send(HttpRequest request, Func<HttpResponse> next)
        {
            AddAuthorizationHeader(request);

            return next();
        }

        private static void AddAuthorizationHeader(HttpRequest request)
        {
            var item = request.Headers.FirstOrDefault(x => x.Name == "Authorization");

            if (item != null)
            {
                request.Headers.Remove(item);
            }

            if (request.Context.ContainsKey("username") && request.Context.ContainsKey("password"))
            {
                var user = request.Context["username"] as string;

                var password = request.Context["password"] as string;

                if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password))
                {
                    request.Headers.Add(new HttpHeader("Authorization", $"{"Basic"} {Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + password))}"));
                }
            }
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request, Func<Task<HttpResponse>> next)
        {
            AddAuthorizationHeader(request);

            return await next();
        }
    }
}
