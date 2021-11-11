using System;
using AutoMapper;
using FluentAssertions;
using Payment.Gateway.Api.StartupConfiguration;
using Xunit;

namespace Payment.Gateway.Api.UnitTests.Mappings
{
    public class PaymentResponseMappingProfileTests
    {
        private readonly IMapper _mapper;

        public PaymentResponseMappingProfileTests()
        {
            _mapper = MappingConfiguration.GetDefaultMapperConfiguration().CreateMapper();
        }

        [Fact]
        public void Map_WhenGivenDatabasePaymentResponse_ShouldReturnMessagingPaymentResponse()
        {
            var response = new Repository.Models.PaymentResponse
            {
                TransactionId = Guid.NewGuid(),
            };

            var mappedResponse = _mapper.Map<Messaging.PaymentResponse>(response);

            mappedResponse.Should().NotBeNull();
            mappedResponse.TransactionId.Should().Be(response.TransactionId);
        }
    }
}
