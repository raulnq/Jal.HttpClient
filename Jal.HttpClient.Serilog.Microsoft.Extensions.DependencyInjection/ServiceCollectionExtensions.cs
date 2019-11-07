using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Model;
using Jal.HttpClient.Serilog;
using Jal.Locator.Microsoft.Extensions.DependencyInjection.Extensions;
using Jal.Locator.Microsoft.Extensions.DependencyInjection.Interface;

namespace Jal.HttpClient.LightInject.Serilog.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static INamedServiceCollection AddSerilogForHttpClient(this INamedServiceCollection servicecollection)
        {
            servicecollection.AddSingleton<IMiddlewareAsync<HttpWrapper>, SerilogMiddelware>(typeof(SerilogMiddelware).FullName);

            return servicecollection;
        }
    }
}
