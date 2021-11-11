using System;
using System.Threading.Tasks;
using Payment.Gateway.Api.Repository.Models;

namespace Payment.Gateway.Api.Repository
{
    /// <summary>
    /// Interface for Payment repository functionality
    /// </summary>
    public interface IPaymentRepository
    {
        /// <summary>
        /// Create a payment request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PaymentResponse> InsertPaymentRequestAsync(Repository.Models.PaymentRequest request);

        /// <summary>
        /// Updates a payment request
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="successIndicator"></param>
        /// <returns></returns>
        Task<PaymentResponse> UpdatePaymentRequestAsync(Guid paymentId, bool successIndicator);

        /// <summary>
        /// Gets details for a particular payment
        /// </summary>
        /// <param name="paymentId">The consumers identifier for the payment</param>
        /// <param name="merchantId">THe identifier for the merchant</param>
        /// <returns></returns>
        Task<PaymentRequest> GetPaymentDetailsAsync(Guid paymentId, Guid merchantId);
    }
}
