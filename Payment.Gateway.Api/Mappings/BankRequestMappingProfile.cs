using AutoMapper;

namespace Payment.Gateway.Api.Mappings
{
    /// <summary>
    /// Used to map from our version of a payment request - to the banks version of a payment request
    /// </summary>
    public class BankRequestMappingProfile : Profile
    {
        /// <summary>
        /// BankRequestMappingProfile
        /// </summary>
        public BankRequestMappingProfile()
        {
            CreateMap<Repository.Models.PaymentRequest, Messaging.BankRequest>();
        }
    }
}
