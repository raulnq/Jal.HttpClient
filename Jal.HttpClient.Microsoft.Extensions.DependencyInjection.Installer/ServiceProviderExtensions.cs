using Microsoft.Extensions.DependencyInjection;
using System;

namespace Jal.HttpClient.Microsoft.Extensions.DependencyInjection.Installer
{
    public static class ServiceProviderExtensions
    {
        public static IHttpFluentHandler GetHttpClient(this IServiceProvider provider)
        {
            return provider.GetService<IHttpFluentHandler>();
        }
    }
}
