using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Payment.Gateway.Api.Enums;
using Payment.Gateway.Api.Exceptions;
using Payment.Gateway.Api.Repository;
using Payment.Gateway.Api.Repository.Models;
using Xunit;

namespace Payment.Gateway.Api.IntegrationTests.Repositories
{
    public class PaymentRepositoryTests : IClassFixture<MongoDbClassFixture>
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentRepositoryTests(MongoDbClassFixture fixture)
        {
            var mongodbContext = fixture.CreateMongoDbContext();
            var logger = new NullLogger<PaymentRepository>();
            this._paymentRepository =  new PaymentRepository(mongodbContext, logger);
        }

        [Fact]
        public async Task GetPaymentDetailsAsync_ForMissingPayment_ThrowsException()
        {
            var paymentGuid = Guid.NewGuid();
            var merchantGuid = Guid.NewGuid();

            Func<Task> getPaymentAsync = async () =>
            {
                await this._paymentRepository.GetPaymentDetailsAsync(paymentGuid, merchantGuid);
            };

            await getPaymentAsync.Should().ThrowAsync<PaymentNotFoundException>();
        }

        [Fact]
        public async Task GetPaymentDetailsAsync_ForExistingPayment_ShouldReturnIt()
        {
            var paymentGuid = Guid.NewGuid();
            var merchantGuid = Guid.NewGuid();
            var transactionGuid = Guid.NewGuid();

            var paymentRequest = new PaymentRequest
            {
                TransactionId = transactionGuid,
                PaymentId = paymentGuid,
                MerchantId = merchantGuid,
                PaymentStatus = PaymentStatus.Pending,
            };

            await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest);

            var response = await this._paymentRepository.GetPaymentDetailsAsync(paymentGuid, merchantGuid);

            response.Should().NotBeNull();
            response.TransactionId.Should().Be(transactionGuid);
            response.PaymentId.Should().Be(paymentGuid);
            response.MerchantId.Should().Be(merchantGuid);
        }

        [Fact]
        public async Task InsertPaymentRequestAsync_ForNewPayment_ShouldInsertIt()
        {
            var paymentGuid = Guid.NewGuid();
            var merchantGuid = Guid.NewGuid();
            var transactionId = Guid.NewGuid();

            var paymentRequest = new PaymentRequest
            {
                PaymentId = paymentGuid,
                MerchantId = merchantGuid,
                PaymentStatus = PaymentStatus.Pending,
                TransactionId = transactionId,
            };

            var response = await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest);
            
            response.Should().NotBeNull();
            response.TransactionId.Should().Be(transactionId);
        }

        [Fact]
        public async Task InsertPaymentRequestAsync_ForExistingPendingPayment_ShouldUpsertIt()
        {
            var paymentGuid = Guid.NewGuid();
            var merchantGuid = Guid.NewGuid();
            var transactionId = Guid.NewGuid();
            var originalCardNumber = 123;

            var paymentRequest = new PaymentRequest
            {
                PaymentId = paymentGuid,
                MerchantId = merchantGuid,
                PaymentStatus = PaymentStatus.Pending,
                TransactionId = transactionId,
                CardNumber = originalCardNumber,
            };

            // This will insert the original record
            var response = await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest);

            response.Should().NotBeNull();
            response.TransactionId.Should().Be(transactionId);

            // This will upsert the pending record and should work
            var updatedCardNumber = 321;
            paymentRequest.CardNumber = updatedCardNumber;
            response = await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest);

            response.Should().NotBeNull();

            // This will ge the latest version of the payment request
            var detailsResponse = await this._paymentRepository.GetPaymentDetailsAsync(paymentGuid, merchantGuid);
            detailsResponse.Should().NotBeNull();
            detailsResponse.TransactionId.Should().Be(transactionId);
            detailsResponse.CardNumber.Should().Be(updatedCardNumber);
        }

        [Theory]
        [InlineData(PaymentStatus.Completed)]
        [InlineData(PaymentStatus.Pending)]
        [InlineData(PaymentStatus.Rejected)]
        public async Task InsertPaymentRequestAsync_ForSamePaymentIdButDifferentMerchants_ShouldInsert(PaymentStatus paymentStatus)
        {
            var paymentGuid = Guid.NewGuid();
            var merchantGuid = Guid.NewGuid();
            var transactionId = Guid.NewGuid();

            var paymentRequest1 = new PaymentRequest
            {
                PaymentId = paymentGuid,
                MerchantId = merchantGuid,
                PaymentStatus = paymentStatus,
                TransactionId = transactionId,
            };

            var response = await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest1);

            response.Should().NotBeNull();
            response.TransactionId.Should().Be(transactionId);

            var transactionId2 = Guid.NewGuid();
            var merchantGuid2 = Guid.NewGuid();

            var paymentRequest2 = new PaymentRequest
            {
                PaymentId = paymentGuid,
                MerchantId = merchantGuid2,
                PaymentStatus = paymentStatus,
                TransactionId = transactionId2,
            };

            response = await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest2);

            response.Should().NotBeNull();
            response.TransactionId.Should().Be(transactionId2);
        }

        [Fact]
        public async Task InsertPaymentRequestAsync_ForExistingCompletedPayment_ShouldThrow()
        {
            var paymentGuid = Guid.NewGuid();
            var merchantGuid = Guid.NewGuid();
            var transactionId = Guid.NewGuid();
            var originalCardNumber = 123;

            var paymentRequest = new PaymentRequest
            {
                PaymentId = paymentGuid,
                MerchantId = merchantGuid,
                PaymentStatus = PaymentStatus.Completed,
                TransactionId = transactionId,
                CardNumber = originalCardNumber,
            };

            // This will insert the original record
            var response = await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest);

            response.Should().NotBeNull();
            response.TransactionId.Should().Be(transactionId);

            // This will try to upsert the completed record and should fail
            var updatedCardNumber = 321;
            paymentRequest.CardNumber = updatedCardNumber;

            Func<Task> insertPaymentAsync = async () =>
            {
                response = await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest);
            };

            await insertPaymentAsync.Should().ThrowAsync<PaymentDuplicateException>();
        }

        [Fact]
        public async Task UpdatePaymentRequestAsync_ForExistingPayment_ShouldUpsertIt()
        {
            var paymentGuid = Guid.NewGuid();
            var merchantGuid = Guid.NewGuid();
            var transactionId = Guid.NewGuid();
            var originalCardNumber = 123;

            var paymentRequest = new PaymentRequest
            {
                PaymentId = paymentGuid,
                MerchantId = merchantGuid,
                PaymentStatus = PaymentStatus.Pending,
                TransactionId = transactionId,
                CardNumber = originalCardNumber,
            };

            // This will insert the original record
            var response = await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest);

            response.Should().NotBeNull();
            response.TransactionId.Should().Be(transactionId);

            // This will update the pending record and should work
            response = await this._paymentRepository.UpdatePaymentRequestAsync(paymentRequest.TransactionId, true);

            response.Should().NotBeNull();
            response.TransactionId.Should().Be(transactionId);
        }

        [Fact]
        public async Task UpdatePaymentRequestAsync_ForMissingPayment_ShouldThrow()
        {
            var transactionId = Guid.NewGuid();

            // This payment will not exist so should throw
            Func<Task> updatePaymentAsync = async () =>
            {
                await this._paymentRepository.UpdatePaymentRequestAsync(transactionId, true);
            };

            await updatePaymentAsync.Should().ThrowAsync<PaymentNotFoundException>();
        }

        [Theory]
        [InlineData(PaymentStatus.Completed)]
        [InlineData(PaymentStatus.Rejected)]
        public async Task UpdatePaymentRequestAsync_ForExistingCompletedPayment_ShouldThrow(PaymentStatus paymentStatus)
        {
            var paymentGuid = Guid.NewGuid();
            var merchantGuid = Guid.NewGuid();
            var transactionId = Guid.NewGuid();
            var originalCardNumber = 123;

            var paymentRequest = new PaymentRequest
            {
                PaymentId = paymentGuid,
                MerchantId = merchantGuid,
                PaymentStatus = paymentStatus,
                TransactionId = transactionId,
                CardNumber = originalCardNumber,
            };

            // This will insert the original record
            var response = await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest);

            response.Should().NotBeNull();
            response.TransactionId.Should().Be(transactionId);

            Func<Task> updatePaymentAsync = async () =>
            {
                await this._paymentRepository.UpdatePaymentRequestAsync(paymentRequest.TransactionId, true);
            };

            await updatePaymentAsync.Should().ThrowAsync<PaymentNotFoundException>();
        }

        [Fact]
        public async Task UpdatePaymentRequestAsync_AttemptToUpdateTwice_ShouldThrow()
        {
            var paymentGuid = Guid.NewGuid();
            var merchantGuid = Guid.NewGuid();
            var transactionId = Guid.NewGuid();
            var originalCardNumber = 123;

            var paymentRequest = new PaymentRequest
            {
                PaymentId = paymentGuid,
                MerchantId = merchantGuid,
                PaymentStatus = PaymentStatus.Pending,
                TransactionId = transactionId,
                CardNumber = originalCardNumber,
            };

            // This will insert the original record
            var insertResponse = await this._paymentRepository.InsertPaymentRequestAsync(paymentRequest);

            insertResponse.Should().NotBeNull();
            insertResponse.TransactionId.Should().Be(transactionId);

            // This should work
            var updateResponse = await this._paymentRepository.UpdatePaymentRequestAsync(paymentRequest.TransactionId, true);
            updateResponse.Should().NotBeNull();

            Func<Task> updatePaymentAsync = async () =>
            {
                await this._paymentRepository.UpdatePaymentRequestAsync(paymentRequest.TransactionId, true);
            };

            // A second update should fail
            await updatePaymentAsync.Should().ThrowAsync<PaymentNotFoundException>();
        }
    }
}
