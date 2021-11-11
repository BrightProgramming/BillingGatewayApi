using System;
using AutoMapper;
using Payment.Gateway.Api.Enums;

namespace Payment.Gateway.Api.Mappings
{
    /// <summary>
    /// Mapping Profile for Payment Requests
    /// </summary>
    public class PaymentRequestMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentRequestMappingProfile()
        {
            CreateMap<Messaging.PaymentRequest, Repository.Models.PaymentRequest>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => PaymentStatus.Pending));
        }
    }
}
