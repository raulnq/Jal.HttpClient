using Jal.ChainOfResponsability;
using System;
using System.Threading.Tasks;

namespace Jal.HttpClient
{
    public class TokenAuthenticatorMiddleware : IAsyncMiddleware<HttpContext>
    {
        public const string TOKEN_VALUE_KEY= "tokenvalue";

        public const string TOKEN_TYPE_KEY = "tokentype";

        private static void AddAuthorizationHeader(HttpRequest request)
        {
            if(request.Context.ContainsKey(TOKEN_VALUE_KEY) && request.Context.ContainsKey(TOKEN_TYPE_KEY))
            {
                var token = request.Context[TOKEN_VALUE_KEY] as string;

                var type = request.Context[TOKEN_TYPE_KEY] as string;

                if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(type))
                {
                    request.Message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"{type}", token);
                }
            }
        }

        public Task ExecuteAsync(AsyncContext<HttpContext> context, Func<AsyncContext<HttpContext>, Task> next)
        {
            AddAuthorizationHeader(context.Data.Request);

            return next(context);
        }
    }
}
