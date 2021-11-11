using AutoMapper;
using Payment.Gateway.Api.Extensions;

namespace Payment.Gateway.Api.Mappings
{
    /// <summary>
    /// Used to map from our database representation of a payment - to a version of the details for the consumer
    /// </summary>
    public class GetPaymentDetailsResponseMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GetPaymentDetailsResponseMappingProfile()
        {
            CreateMap<Repository.Models.PaymentRequest, Messaging.GetPaymentDetailsResponse>()
                .ForMember(dest => dest.SuccessIndicator, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.CardNumber.ToString().Mask('*', 8)))
                .ForMember(dest => dest.ExpiryMonth, opt => opt.MapFrom(src => src.ExpiryMonth.ToString().Mask('*', 0)))
                .ForMember(dest => dest.ExpiryYear, opt => opt.MapFrom(src => src.ExpiryYear.ToString().Mask('*', 0)))
                .ForMember(dest => dest.Cvv, opt => opt.MapFrom(src => src.Cvv.ToString().Mask('*', 0)))
                ;

        }
    }
}
