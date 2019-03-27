using System;
using System.Text;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{

    public class BasicHttpAuthenticatorMiddleware : IMiddlewareAsync<HttpWrapper>
    {
        private void AddAuthorizationHeader(HttpRequest request)
        {
            if (request.Context.ContainsKey("username") && request.Context.ContainsKey("password"))
            {
                var user = request.Context["username"] as string;

                var password = request.Context["password"] as string;

                if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password))
                {
                    request.Message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + password)));
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
