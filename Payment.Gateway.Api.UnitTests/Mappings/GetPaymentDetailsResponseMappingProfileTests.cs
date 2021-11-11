using System;
using AutoMapper;
using FluentAssertions;
using Payment.Gateway.Api.Enums;
using Payment.Gateway.Api.Messaging;
using Payment.Gateway.Api.StartupConfiguration;
using Xunit;

namespace Payment.Gateway.Api.UnitTests.Mappings
{
    public class GetPaymentDetailsResponseMappingProfileTests
    {
        private readonly IMapper _mapper;

        public GetPaymentDetailsResponseMappingProfileTests()
        {
            _mapper = MappingConfiguration.GetDefaultMapperConfiguration().CreateMapper();
        }

        [Fact]
        public void Map_WhenGivenPaymentRequest_MapsCorrectlyToGetPaymentDetailsResponse()
        {
            var request = new Repository.Models.PaymentRequest
            {
                PaymentId = Guid.NewGuid(),
                CardNumber = 1234567890123456,
                Amount = 12,
                Cvv = 123,
                ExpiryMonth = 12,
                ExpiryYear = 2021,
                PaymentStatus = PaymentStatus.Completed,
            };
            
            var mappedResponse = _mapper.Map<GetPaymentDetailsResponse>(request);

            mappedResponse.Should().NotBeNull();
            mappedResponse.PaymentId.Should().Be(request.PaymentId);
            mappedResponse.CardNumber.Should().Be("12345678********");
            mappedResponse.Cvv.Should().Be("***");
            mappedResponse.ExpiryMonth.Should().Be("**");
            mappedResponse.ExpiryYear.Should().Be("****");
            mappedResponse.SuccessIndicator.Should().Be(PaymentStatus.Completed.ToString());

        }
    }
}
