using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jal.HttpClient.Impl
{
    public class TokenAuthenticatorMiddleware : IHttpMiddleware
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

            if(request.Context.ContainsKey("tokenvalue") && request.Context.ContainsKey("tokentype"))
            {
                var token = request.Context["tokenvalue"] as string;

                var type = request.Context["tokentype"] as string;

                if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(type))
                {
                    request.Headers.Add(new HttpHeader("Authorization", $"{type} {token}"));
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
