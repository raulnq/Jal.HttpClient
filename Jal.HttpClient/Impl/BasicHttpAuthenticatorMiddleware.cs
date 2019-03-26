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
        public HttpResponse Send(HttpRequest request, Func<HttpRequest, HttpContext, HttpResponse> next, HttpContext context)
        {
            AddAuthorizationHeader(request);

            return next(request, context);
        }

        private static void AddAuthorizationHeader(HttpRequest request)
        {
            if (request.Headers.Contains("Authorization"))
            {
                request.Headers.Remove("Authorization");
            }

            if (request.Context.ContainsKey("username") && request.Context.ContainsKey("password"))
            {
                var user = request.Context["username"] as string;

                var password = request.Context["password"] as string;

                if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password))
                {
                    request.Headers.Add("Authorization", $"{"Basic"} {Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + password))}");
                }
            }
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request, Func<HttpRequest, HttpContext, Task<HttpResponse>> next, HttpContext context)
        {
            AddAuthorizationHeader(request);

            return await next(request, context);
        }
    }
}
