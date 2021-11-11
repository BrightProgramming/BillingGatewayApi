using System;
using System.Threading.Tasks;
using Payment.Gateway.Api.Messaging;

namespace Payment.Gateway.Api.Services
{
    /// <summary>
    /// Interface for payment services
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Process a payment
        /// </summary>
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);

        /// <summary>
        /// Get details for a particular payment
        /// </summary>
        Task<GetPaymentDetailsResponse> GetPaymentDetailsAsync(Guid paymentId, Guid merchantId);
    }
}
