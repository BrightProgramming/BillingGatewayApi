using System;

namespace Banking.Simulator.Api.Messaging
{
    /// <summary>
    /// Response from the payment simulator
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// A unique identifier for the response
        /// </summary>
        public Guid ResponseId { get; set; }
 
        /// <summary>
        /// Transaction Id
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Success Indicator
        /// </summary>
        public bool Success { get; set; }
    }
}