using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Gateway.Api.Configuration;
using Payment.Gateway.Api.Extensions;
using Payment.Gateway.Api.Repository;
using Payment.Gateway.Api.Security;
using Payment.Gateway.Api.Services;

namespace Payment.Gateway.Api.StartupConfiguration
{
    /// <summary>
    /// Manages application configuration
    /// </summary>
    public static class ApplicationConfiguration
    {
        /// <summary>
        /// Add application dependency injection
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var bankServiceConfig = configuration.BindConfigurationSection<BankServiceConfig>("BankServiceConfig");
            services.AddSingleton(bankServiceConfig);

            var securityConfig = configuration.BindConfigurationSection<SecurityConfig>("SecurityConfig");
            services.AddSingleton(securityConfig);

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<IPaymentRepository, PaymentRepository>();
            services.AddScoped<AddAuthenticatedUserHeaderHandler>();

            return services;
        }
    }
}
