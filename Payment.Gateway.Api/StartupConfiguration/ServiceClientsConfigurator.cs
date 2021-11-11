using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Payment.Gateway.Api.Security;
using Payment.Gateway.Api.Services;

namespace Payment.Gateway.Api.StartupConfiguration
{
    /// <summary>
    /// Configurator for the service client - for making api calls to the banking service.
    /// </summary>
    public static class ServiceClientsConfigurator
    {
        /// <summary>
        /// Add a HttpClient
        /// </summary>
        public static IServiceCollection AddServiceClients(this IServiceCollection serviceCollection, string baseUrl)
        {
            ConfigureHttpClientFor<IBankServiceClient, BankServiceClient>(serviceCollection, baseUrl);

            return serviceCollection;
        }

        private static void ConfigureHttpClientFor<T, TImplementation>(IServiceCollection services, string baseUrl)
            where TImplementation : class, T where T : class
        {
            services.AddHttpClient<T, TImplementation>(c =>
                {
                    c.BaseAddress = new Uri(baseUrl);
                    c.Timeout = TimeSpan.FromSeconds(30);
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler
                    {
                        ClientCertificateOptions = ClientCertificateOption.Manual,
                        ServerCertificateCustomValidationCallback =
                            (httpRequestMessage, cert, certChain, policyErrors) => true
                    };
                })
                .AddHttpMessageHandler<AddAuthenticatedUserHeaderHandler>();
        }
    }
}
