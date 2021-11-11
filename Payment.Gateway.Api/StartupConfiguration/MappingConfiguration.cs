using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Payment.Gateway.Api.Mappings;

namespace Payment.Gateway.Api.StartupConfiguration
{
    /// <summary>
    /// Mapping configuration
    /// </summary>
    public static class MappingConfiguration
    {
        /// <summary>
        /// Add mappings
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddSingleton(GetDefaultMapperConfiguration().CreateMapper());
            return services;
        }

        /// <summary>
        /// Gets the default mapper configuration
        /// </summary>
        public static MapperConfiguration GetDefaultMapperConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PaymentRequestMappingProfile>();
                cfg.AddProfile<BankRequestMappingProfile>();
                cfg.AddProfile<PaymentResponseMappingProfile>();
                cfg.AddProfile<GetPaymentDetailsResponseMappingProfile>();
            });
        }
    }
}
