using System;
using System.Text;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{

    public class BasicHttpAuthenticatorMiddleware : IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>
    {
        private static void AddAuthorizationHeader(HttpRequest request)
        {
            if (request.Context.ContainsKey("username") && request.Context.ContainsKey("password"))
            {
                var user = request.Context["username"] as string;

                var password = request.Context["password"] as string;

                if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + password)));
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
