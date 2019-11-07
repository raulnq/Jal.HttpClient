using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Impl.Fluent;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;
using Jal.Locator.Microsoft.Extensions.DependencyInjection.Interface;
using Microsoft.Extensions.DependencyInjection;
using Jal.Locator.Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jal.HttpClient.Microsoft.Extensions.DependencyInjection.Installer
{
    public static class ServiceCollectionExtensions
    {
        public static INamedServiceCollection AddHttpClient(this INamedServiceCollection servicecollection)
        {
            servicecollection.ServiceCollection.AddSingleton<IHttpFluentHandler, HttpFluentHandler>();

            servicecollection.ServiceCollection.AddSingleton<IHttpHandler, HttpHandler>();

            servicecollection.AddSingleton<IMiddlewareAsync<HttpWrapper>, BasicHttpAuthenticatorMiddleware>(typeof(BasicHttpAuthenticatorMiddleware).FullName);

            servicecollection.AddSingleton<IMiddlewareAsync<HttpWrapper>, HttpMiddelware>(typeof(HttpMiddelware).FullName);

            servicecollection.AddSingleton<IMiddlewareAsync<HttpWrapper>, TokenAuthenticatorMiddleware>(typeof(TokenAuthenticatorMiddleware).FullName);

            servicecollection.AddSingleton<IMiddlewareAsync<HttpWrapper>, MemoryCacheMiddleware>(typeof(MemoryCacheMiddleware).FullName);

            servicecollection.AddSingleton<IMiddlewareAsync<HttpWrapper>, IdentityTrackerMiddleware>(typeof(IdentityTrackerMiddleware).FullName);

            return servicecollection;
        }
    }
}
