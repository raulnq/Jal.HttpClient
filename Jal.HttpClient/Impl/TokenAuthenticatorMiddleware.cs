using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.HttpClient.Model;
using System;
using System.Threading.Tasks;

namespace Jal.HttpClient.Impl
{
    public class TokenAuthenticatorMiddleware : IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>
    {
        private static void AddAuthorizationHeader(HttpRequest request)
        {
            if(request.Context.ContainsKey("tokenvalue") && request.Context.ContainsKey("tokentype"))
            {
                var token = request.Context["tokenvalue"] as string;

                var type = request.Context["tokentype"] as string;

                if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(type))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"{type}", token);
                }
            }
        }

        public void Execute(Context<HttpMessageWrapper> context, Action<Context<HttpMessageWrapper>> next)
        {
            AddAuthorizationHeader(context.Data.Request);

            next(context);
        }

        public Task ExecuteAsync(Context<HttpMessageWrapper> context, Func<Context<HttpMessageWrapper>, Task> next)
        {
            AddAuthorizationHeader(context.Data.Request);

            return next(context);
        }
    }
}
