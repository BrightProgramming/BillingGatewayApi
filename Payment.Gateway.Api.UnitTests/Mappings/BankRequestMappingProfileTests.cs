using System;
using AutoMapper;
using FluentAssertions;
using Payment.Gateway.Api.Messaging;
using Payment.Gateway.Api.StartupConfiguration;
using Xunit;

namespace Payment.Gateway.Api.UnitTests.Mappings
{
    public class BankRequestMappingProfileTests
    {
        private readonly IMapper _mapper;
        public BankRequestMappingProfileTests()
        {
            _mapper = MappingConfiguration.GetDefaultMapperConfiguration().CreateMapper();
        }

        [Fact]
        public void Map_WhenGivenPaymentRequest_ShouldMapToBankRequest()
        {
            var request = new Repository.Models.PaymentRequest
            {
                TransactionId = Guid.NewGuid(),
            };

            var mappedRequest = _mapper.Map<BankRequest>(request);

            mappedRequest.TransactionId.Should().Be(request.TransactionId);
        }
    }
}
