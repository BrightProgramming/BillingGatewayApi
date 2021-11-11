using System;
using AutoMapper;
using FluentAssertions;
using Payment.Gateway.Api.StartupConfiguration;
using Xunit;

namespace Payment.Gateway.Api.UnitTests.Mappings
{
    public class PaymentRequestMappingProfileTests
    {
        private readonly IMapper _mapper;

        public PaymentRequestMappingProfileTests()
        {
            _mapper = MappingConfiguration.GetDefaultMapperConfiguration().CreateMapper();
        }

        [Fact]
        public void Map_WhenGivenMessagingPaymentRequest_ShouldReturnDatabasePaymentRequest()
        {
            var request = new Messaging.PaymentRequest
            {
                PaymentId = Guid.NewGuid(),
            };

            var mappedRequest = _mapper.Map<Repository.Models.PaymentRequest>(request);

            mappedRequest.Should().NotBeNull();
            mappedRequest.PaymentId.Should().Be(request.PaymentId);
        }
    }
}
