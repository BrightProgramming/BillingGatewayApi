using System;

namespace Payment.Gateway.Api.Messaging
{
    /// <summary>
    /// Our response to a payment - for sending to our consumer
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Our unique identifier we give the consumer for the transaction
        /// </summary>
        public Guid TransactionId { get; set; } 

        /// <summary>
        /// Our indicator of transaction success
        /// </summary>
        public string PaymentStatus { get; set; }
    }
}
