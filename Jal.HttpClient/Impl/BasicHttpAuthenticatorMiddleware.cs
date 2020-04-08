using System;
using System.Text;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient
{

    public class BasicHttpAuthenticatorMiddleware : IAsyncMiddleware<HttpContext>
    {
        public const string USER_NAME_KEY = "username";

        public const string PASSWORD_KEY = "password";

        private void AddAuthorizationHeader(HttpRequest request)
        {
            if (request.Context.ContainsKey(USER_NAME_KEY) && request.Context.ContainsKey(PASSWORD_KEY))
            {
                var user = request.Context[USER_NAME_KEY] as string;

                var password = request.Context[PASSWORD_KEY] as string;

                if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(password))
                {
                    request.Message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + password)));
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
