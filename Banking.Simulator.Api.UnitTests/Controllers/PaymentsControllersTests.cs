using System.Threading.Tasks;
using Banking.Simulator.Api.Controllers;
using Banking.Simulator.Api.Messaging;
using Banking.Simulator.Api.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Banking.Simulator.Api.UnitTests.Controllers
{
    public class PaymentsControllersTests
    {
        [Fact]
        public async Task ProcessPayment_ForValidRequest_ShouldCallServicesAsExpected()
        {
            var paymentServiceMock = new Mock<IPaymentProcessorService>();
            paymentServiceMock.Setup(x => x.ValidatePaymentAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(new PaymentResponse()).Verifiable();

            var paymentController = new PaymentController(paymentServiceMock.Object);

            var request = new PaymentRequest();

            var response = await paymentController.ProcessPayment(request);

            response.Should().NotBeNull();
            paymentServiceMock.Verify(x => x.ValidatePaymentAsync(It.IsAny<PaymentRequest>()), Times.Once);
        }
    }
}
