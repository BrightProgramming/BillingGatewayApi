using System.Threading.Tasks;
using Banking.Simulator.Api.Messaging;
using Banking.Simulator.Api.Services;
using FluentAssertions;
using Xunit;

namespace Banking.Simulator.Api.UnitTests.Services
{
    public class PaymentProcessorServiceTests
    {
        [Theory]
        [InlineData(1234567891234566, true)]
        [InlineData(1234567891234565, false)]
        public async Task ValidatePaymentAsync_WhenGivenCardNumber_ShouldReturnExpectedResponse(long cardNumber, bool expectedResult)
        {
            var request = new PaymentRequest
            {
                CardNumber = cardNumber,
            };

            var paymentProcessor = new PaymentProcessorService();
            var response = await paymentProcessor.ValidatePaymentAsync(request);

            response.Success.Should().Be(expectedResult);
        }
    }
}
