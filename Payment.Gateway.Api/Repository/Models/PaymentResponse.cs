using System;
using Payment.Gateway.Api.Enums;

namespace Payment.Gateway.Api.Repository.Models
{
    /// <summary>
    /// DB response for a payment request
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// The unique identifier used by payments for this transaction
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// The status of the payment
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }
    }
}
