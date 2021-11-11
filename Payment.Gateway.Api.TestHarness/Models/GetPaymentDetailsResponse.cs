using System;

namespace Payment.Gateway.Api.TestHarness.Models
{
    public class GetPaymentDetailsResponse
    {
        /// <summary>
        /// The identifier for the merchant
        /// </summary>
        public Guid PaymentId { get; set; }

        /// <summary>
        /// The identifier for the merchant
        /// </summary>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// The card number
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// The expiry month
        /// </summary>
        public string ExpiryMonth { get; set; }

        /// <summary>
        /// The expiry year
        /// </summary>
        public string ExpiryYear { get; set; }

        /// <summary>
        /// The payment amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The Cvv
        /// </summary>
        public string Cvv { get; set; }

        /// <summary>
        /// The transaction date
        /// </summary>
        public DateTimeOffset TransactionDate { get; set; }

        /// <summary>
        /// Was the payment successful
        /// </summary>
        public string SuccessIndicator { get; set; }
    }
}
