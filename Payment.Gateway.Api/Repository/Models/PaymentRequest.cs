using System;
using MongoDB.Bson.Serialization.Attributes;
using Payment.Gateway.Api.Enums;

namespace Payment.Gateway.Api.Repository.Models
{
    /// <summary>
    /// Database model for a Payment Request
    /// </summary>
    public class PaymentRequest
    {
        /// <summary>
        /// Our unique identifier from the transaction        /// </summary>
        [BsonId]
        public Guid TransactionId { get; set; }

        /// <summary>
        /// The identifier from the merchant
        /// </summary>
        public Guid PaymentId { get; set; }

        /// <summary>
        /// The identifier for the merchant
        /// </summary>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// The card number
        /// </summary>
        public long CardNumber { get; set; }

        /// <summary>
        /// The expiry month
        /// </summary>
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// The expiry year
        /// </summary>
        public int ExpiryYear { get; set; }

        /// <summary>
        /// The payment amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The Cvv
        /// </summary>
        public int Cvv { get; set; }

        /// <summary>
        /// The transaction date
        /// </summary>
        public DateTimeOffset TransactionDate { get; set; }

        /// <summary>
        /// The payment status
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }
    }
}
