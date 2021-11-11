using AutoMapper;

namespace Payment.Gateway.Api.Mappings
{
    /// <summary>
    /// Used to map from our database version of a payment response - to a consumer version of  payment response
    /// </summary>
    public class PaymentResponseMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentResponseMappingProfile()
        {
            CreateMap<Repository.Models.PaymentResponse, Messaging.PaymentResponse>()
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString())).ReverseMap();
        }
    }
}
