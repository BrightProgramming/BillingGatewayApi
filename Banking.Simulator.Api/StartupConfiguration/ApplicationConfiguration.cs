using Banking.Simulator.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Simulator.Api.StartupConfiguration
{
    /// <summary>
    /// Manages application configuration
    /// </summary>
    public static class ApplicationConfiguration
    {
        /// <summary>
        /// Add application dependency injection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IPaymentProcessorService, PaymentProcessorService>();
            services.AddSingleton<IUserService, UserService>();

            return services;
        }
    }
}
