using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Payment.Gateway.Api.Messaging;
using Payment.Gateway.Api.Validation;

namespace Payment.Gateway.Api.StartupConfiguration
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
