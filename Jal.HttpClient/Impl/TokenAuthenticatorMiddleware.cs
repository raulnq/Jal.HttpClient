using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.HttpClient.Model;
using System;
using System.Threading.Tasks;

namespace Jal.HttpClient.Impl
{
    public class TokenAuthenticatorMiddleware : IMiddlewareAsync<HttpWrapper>
    {
        private static void AddAuthorizationHeader(HttpRequest request)
        {
            if(request.Context.ContainsKey("tokenvalue") && request.Context.ContainsKey("tokentype"))
            {
                var token = request.Context["tokenvalue"] as string;

                var type = request.Context["tokentype"] as string;

                if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(type))
                {
                    request.Message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"{type}", token);
                }
            }
        }

        public Task ExecuteAsync(Context<HttpWrapper> context, Func<Context<HttpWrapper>, Task> next)
        {
            AddAuthorizationHeader(context.Data.Request);

            return next(context);
        }
    }
}
