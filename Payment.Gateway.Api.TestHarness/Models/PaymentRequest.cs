using System;

namespace Payment.Gateway.Api.TestHarness.Models
{
    public class PaymentRequest
    {
        /// <summary>
        /// The identifier for the payment
        /// </summary>
        /// <example>12345678-9861-4C71-9B9F-201EB65E49D0</example>
        public Guid PaymentId { get; set; }

        /// <summary>
        /// The identifier for the merchant
        /// </summary>
        /// <example>2CB14EAD-9861-4C71-9B9F-201EB65E49D0</example>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// The card number
        /// </summary>
        /// <example>1234123412341234</example>
        public long CardNumber { get; set; }

        /// <summary>
        /// The expiry month
        /// </summary>
        /// <example>12</example>
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// The expiry year
        /// </summary>
        /// <example>2023</example>
        public int ExpiryYear { get; set; }

        /// <summary>
        /// The payment amount
        /// </summary>
        /// <example>10.15</example>
        public decimal Amount { get; set; }

        /// <summary>
        /// The Cvv
        /// </summary>
        /// <example>123</example>
        public int Cvv { get; set; }

        /// <summary>
        /// The transaction date
        /// </summary>
        /// <example>2021-10-29T12:05:08.3587598+00:00</example>
        public DateTimeOffset TransactionDate { get; set; }
    }
}
