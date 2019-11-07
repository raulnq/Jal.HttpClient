using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Common.Logging;
using Jal.HttpClient.Model;
using Jal.Locator.Microsoft.Extensions.DependencyInjection.Extensions;
using Jal.Locator.Microsoft.Extensions.DependencyInjection.Interface;

namespace Jal.HttpClient.Common.Logging.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static INamedServiceCollection AddCommonLoggingForHttpClient(this INamedServiceCollection servicecollection)
        {
            servicecollection.AddSingleton<IMiddlewareAsync<HttpWrapper>, CommonLoggingMiddelware>(typeof(CommonLoggingMiddelware).FullName);

            return servicecollection;
        }
    }
}
