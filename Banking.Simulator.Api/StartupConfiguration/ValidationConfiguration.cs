using Banking.Simulator.Api.Messaging;
using Banking.Simulator.Api.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Simulator.Api.StartupConfiguration
{
    /// <summary>
    /// Validation Configuration
    /// </summary>
    public static class ValidationConfiguration
    {
        /// <summary>
        /// Add fluent validation
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidation();
            services.AddTransient<IValidator<PaymentRequest>, PaymentRequestValidator>();

            return services;
        }
    }
}
