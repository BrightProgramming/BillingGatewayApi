using System;

namespace Payment.Gateway.Api.Messaging
{
    /// <summary>
    /// Our bnking request - to send to the banking provider
    /// </summary>
    public class BankRequest
    {
        /// <summary>
        /// The unique identifier used by payments for the transaction
        /// </summary>
        public Guid TransactionId { get; set; }

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

    }
}
