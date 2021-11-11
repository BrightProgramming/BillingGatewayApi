using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Payment.Gateway.Api.Enums;
using Payment.Gateway.Api.Exceptions;
using Payment.Gateway.Api.Repository.Models;

namespace Payment.Gateway.Api.Repository
{
    /// <summary>
    /// Repository functionality for payments
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMongoDbContext _mongoDbContext;
        private readonly ILogger<PaymentRepository> _logger;

        private const int MongoDbDuplicateKeyError = 11000;

        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentRepository(IMongoDbContext mongoDbContext, ILogger<PaymentRepository> logger)
        {
            _mongoDbContext = mongoDbContext;
            _logger = logger;
        }

        /// <summary>
        /// Inserts a payment request with a status of Pending
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PaymentResponse> InsertPaymentRequestAsync(PaymentRequest request)
        {
            var findFilterPaymentId = Builders<PaymentRequest>.Filter.Eq(x => x.PaymentId, request.PaymentId);
            var findFilterMerchantId = Builders<PaymentRequest>.Filter.Eq(x => x.MerchantId, request.MerchantId);
            var findFilterPaymentStatus = Builders<PaymentRequest>.Filter.Eq(x => x.PaymentStatus, PaymentStatus.Pending); // Can't update a payment if it has been accepted or rejected.

            var options = new FindOneAndReplaceOptions<PaymentRequest>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After,
            };

            try
            {
                var result = await _mongoDbContext.PaymentRequestCollection.FindOneAndReplaceAsync(findFilterPaymentId & findFilterMerchantId & findFilterPaymentStatus, request, options);

                return new PaymentResponse
                {
                    TransactionId = result.TransactionId,
                    PaymentStatus = result.PaymentStatus,
                };
            }
            catch (MongoCommandException ex)
            {
                this._logger.LogError($"Failed to write to mongodb - error {ex.Message}");

                switch (ex.Code)
                {
                    case MongoDbDuplicateKeyError:
                        throw new PaymentDuplicateException("Mongo Db DuplicateKey Error", ex);
                    default:
                        throw;
                }
            }
            catch (MongoWriteException ex)
            {
                this._logger.LogError($"Failed to write to mongodb - error {ex.Message}");

                switch (ex.WriteError.Code)
                {
                    case MongoDbDuplicateKeyError:
                        throw new PaymentDuplicateException("Mongo Db Duplicate Key Error", ex);
                    default:
                        throw;
                }
            }
            catch (Exception ex)
            {

                // Potentially use Polly or equivalent to allow retries.
                this._logger.LogError($"Failed to write to mongodb - error {ex.Message}");

                // rethrow to allow the global exception handler to catch
                throw;
            }
        }

        /// <summary>
        /// Updates a payment request to a status of Approved or Rejected
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="successIndicator"></param>
        /// <returns></returns>
        public async Task<PaymentResponse> UpdatePaymentRequestAsync(Guid transactionId, bool successIndicator)
        {
            var findFilterTransactionId = Builders<PaymentRequest>.Filter.Eq(x => x.TransactionId, transactionId);
            var findFilterPaymentStatus = Builders<PaymentRequest>.Filter.Eq(x => x.PaymentStatus, PaymentStatus.Pending); // Can't update a payment if it has been accepted or rejected.

            var updateFilter = Builders<PaymentRequest>.Update.Set(x =>
                x.PaymentStatus, successIndicator ? PaymentStatus.Completed : PaymentStatus.Rejected);

            var options = new FindOneAndUpdateOptions<PaymentRequest>
            {
                ReturnDocument = ReturnDocument.After,
            };

            try
            {
                var result = await _mongoDbContext.PaymentRequestCollection.FindOneAndUpdateAsync(findFilterTransactionId & findFilterPaymentStatus, updateFilter, options);

                if (result == null)
                {
                    throw new PaymentNotFoundException();
                }

                return new PaymentResponse
                {
                    TransactionId = result.TransactionId,
                    PaymentStatus = result.PaymentStatus,
                };
            }
            catch (Exception ex)
            {

                // Potentially use Polly or equivalent to allow retries.
                this._logger.LogError($"Failed to write to mongodb - error {ex.Message}");

                // rethrow to allow the global exception handler to catch
                throw;
            }
        }

        /// <summary>
        /// The consumer will give us their identifier - not ours
        /// </summary>
        /// <param name="paymentId">the consumers identifier for the transaction</param>
        /// <param name="merchantId">The merchants identifier - so they can only view their own transactions</param>
        /// <returns></returns>
        public async Task<PaymentRequest> GetPaymentDetailsAsync(Guid paymentId, Guid merchantId)
        {
            var payment = await _mongoDbContext.PaymentRequestCollection.Find(x => x.PaymentId == paymentId && x.MerchantId == merchantId).FirstOrDefaultAsync();

            if (payment == null)
            {
                throw new PaymentNotFoundException();
            }

            return payment;
        }
    }
}
