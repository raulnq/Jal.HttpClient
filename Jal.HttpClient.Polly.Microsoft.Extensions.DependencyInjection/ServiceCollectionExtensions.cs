using Jal.ChainOfResponsability.Intefaces;
using Jal.HttpClient.Model;
using Jal.HttpClient.Polly;
using Jal.Locator.Microsoft.Extensions.DependencyInjection.Extensions;
using Jal.Locator.Microsoft.Extensions.DependencyInjection.Interface;
using System;

namespace Jal.HttpClient.LightInject.Polly.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static INamedServiceCollection AddPollyForHttpClient(this INamedServiceCollection servicecollection)
        {
            servicecollection.AddSingleton<IMiddlewareAsync<HttpWrapper>, OnConditionRetryMiddelware>(typeof(OnConditionRetryMiddelware).FullName);

            servicecollection.AddSingleton<IMiddlewareAsync<HttpWrapper>, CircuitBreakerMiddelware>(typeof(CircuitBreakerMiddelware).FullName);

            return servicecollection;
        }
    }
}
