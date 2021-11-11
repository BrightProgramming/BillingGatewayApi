using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Payment.Gateway.Api.Controllers;
using Payment.Gateway.Api.Messaging;
using Payment.Gateway.Api.Services;
using Xunit;

namespace Payment.Gateway.Api.UnitTests.Controllers
{
    public class PaymentControllerTests
    {
        [Fact]
        public async Task ProcessPayment_ForValidRequest_ShouldCallServicesAsExpected()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.ProcessPaymentAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(new PaymentResponse()).Verifiable();
            var logger = new NullLogger<PaymentController>();

            var paymentController = new PaymentController(paymentServiceMock.Object, logger);

            var request = new PaymentRequest();

            var response = await paymentController.ProcessPayment(request);

            response.Should().NotBeNull();
            paymentServiceMock.Verify(x => x.ProcessPaymentAsync(It.IsAny<PaymentRequest>()), Times.Once);
        }

        [Fact]
        public async Task GetPaymentDetails_ForValidRequest_ShouldCallServicesAsExpected()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            var paymentGuid = Guid.NewGuid();
            var merchantGuid = Guid.NewGuid();

            paymentServiceMock.Setup(x => x.GetPaymentDetailsAsync(It.Is<Guid>(y => y == paymentGuid), It.Is<Guid>(z => z == merchantGuid))).ReturnsAsync(new GetPaymentDetailsResponse()).Verifiable();
            var logger = new NullLogger<PaymentController>();

            var paymentController = new PaymentController(paymentServiceMock.Object, logger);

            var request = new PaymentRequest();

            var response = await paymentController.GetPaymentDetails(paymentGuid.ToString(), merchantGuid.ToString());

            response.Should().NotBeNull();
            paymentServiceMock.Verify(x => x.GetPaymentDetailsAsync(It.Is<Guid>(y => y == paymentGuid), It.Is<Guid>(z => z == merchantGuid)), Times.Once);
        }
    }
}
