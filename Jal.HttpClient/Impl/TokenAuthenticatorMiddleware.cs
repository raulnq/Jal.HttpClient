using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jal.HttpClient.Impl
{
    public class TokenAuthenticatorMiddleware : IHttpMiddleware
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

            if(request.Context.ContainsKey("tokenvalue") && request.Context.ContainsKey("tokentype"))
            {
                var token = request.Context["tokenvalue"] as string;

                var type = request.Context["tokentype"] as string;

                if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(type))
                {
                    request.Headers.Add("Authorization", $"{type} {token}");
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
