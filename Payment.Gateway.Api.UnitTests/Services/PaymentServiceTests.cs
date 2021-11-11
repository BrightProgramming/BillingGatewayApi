using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Payment.Gateway.Api.Enums;
using Payment.Gateway.Api.Exceptions;
using Payment.Gateway.Api.Messaging;
using Payment.Gateway.Api.Repository;
using Payment.Gateway.Api.Services;
using Payment.Gateway.Api.StartupConfiguration;
using Xunit;

namespace Payment.Gateway.Api.UnitTests.Services
{
    public class PaymentServiceTests
    {
        private readonly IMapper _mapper;

        public PaymentServiceTests()
        {
            _mapper = MappingConfiguration.GetDefaultMapperConfiguration().CreateMapper();
        }

        [Fact]
        public async Task GetPaymentDetailsAsync_ForFoundPaymentRequest_ShouldReturnMappedVersion()
        {
            var paymentRepositoryMock = new Mock<IPaymentRepository>();
            var paymentRequestMock = new Repository.Models.PaymentRequest
            {
                PaymentId = Guid.NewGuid(),
                Amount = 12,
                CardNumber = 1234567890123456,
                Cvv = 123,
                ExpiryMonth = 12,
                ExpiryYear = 2021,
                MerchantId = Guid.NewGuid(),
                PaymentStatus = PaymentStatus.Completed,
                TransactionDate = DateTimeOffset.Now,
                TransactionId = Guid.NewGuid(),
            };
            paymentRepositoryMock.Setup(x => x.GetPaymentDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(paymentRequestMock).Verifiable();
            
            var bankServiceClientMock = new Mock<IBankServiceClient>();

            var paymentService = new PaymentService(paymentRepositoryMock.Object, bankServiceClientMock.Object, this._mapper, new NullLogger<PaymentService>());
            var response = await paymentService.GetPaymentDetailsAsync(Guid.NewGuid(), Guid.NewGuid());

            paymentRepositoryMock.Verify(x => x.GetPaymentDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            response.Should().NotBeNull();
            response.PaymentId.Should().Be(paymentRequestMock.PaymentId);
            response.SuccessIndicator.Should().Be(paymentRequestMock.PaymentStatus.ToString());
        }

        [Fact]
        public async Task GetPaymentDetailsAsync_ForMissingPaymentRequest_ShouldThrowException()
        {
            var paymentRepositoryMock = new Mock<IPaymentRepository>();
            paymentRepositoryMock.Setup(x => x.GetPaymentDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Throws(new PaymentNotFoundException());

            var bankServiceClientMock = new Mock<IBankServiceClient>();

            var paymentService = new PaymentService(paymentRepositoryMock.Object, bankServiceClientMock.Object, this._mapper, new NullLogger<PaymentService>());
            Func<Task> getPaymentAsync = async () =>
            {
                await paymentService.GetPaymentDetailsAsync(Guid.NewGuid(), Guid.NewGuid());
            };

            await getPaymentAsync.Should().ThrowAsync<PaymentNotFoundException>();
        }

        [Fact]
        public async Task ProcessPaymentAsync_ForValidPayment_ShouldCallDependenciesAsExpected()
        {
            var paymentRepositoryMock = new Mock<IPaymentRepository>();
            var paymentRequest = new Messaging.PaymentRequest
            {
                PaymentId = Guid.NewGuid(),
                Amount = 12,
                CardNumber = 1234567890123456,
                Cvv = 123,
                ExpiryMonth = 12,
                ExpiryYear = 2021,
                MerchantId = Guid.NewGuid(),
                TransactionDate = DateTimeOffset.Now,
            };
            paymentRepositoryMock.Setup(x => x.InsertPaymentRequestAsync(It.IsAny<Repository.Models.PaymentRequest>())).ReturnsAsync(new Repository.Models.PaymentResponse()).Verifiable();
            var updatedPaymentRequest = new Repository.Models.PaymentResponse
            {
                TransactionId = Guid.NewGuid(),
                PaymentStatus = PaymentStatus.Completed,
            };
            paymentRepositoryMock.Setup(x => x.UpdatePaymentRequestAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(updatedPaymentRequest).Verifiable();

            var bankServiceClientMock = new Mock<IBankServiceClient>();
            bankServiceClientMock.Setup(x => x.ProcessPaymentAsync(It.IsAny<BankRequest>())).ReturnsAsync(new BankResponse()).Verifiable();

            var paymentService = new PaymentService(paymentRepositoryMock.Object, bankServiceClientMock.Object, this._mapper, new NullLogger<PaymentService>());
            var response = await paymentService.ProcessPaymentAsync(paymentRequest);

            paymentRepositoryMock.Verify(x => x.InsertPaymentRequestAsync(It.IsAny<Repository.Models.PaymentRequest>()), Times.Once());
            paymentRepositoryMock.Verify(x => x.UpdatePaymentRequestAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once());
            bankServiceClientMock.Verify(x => x.ProcessPaymentAsync(It.IsAny<BankRequest>()), Times.Once());

            response.Should().NotBeNull();
            response.TransactionId.Should().Be(updatedPaymentRequest.TransactionId);
            response.PaymentStatus.Should().Be(updatedPaymentRequest.PaymentStatus.ToString());
        }
    }
}
