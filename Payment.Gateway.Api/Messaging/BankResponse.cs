using System;

namespace Payment.Gateway.Api.Messaging
{
    /// <summary>
    /// Our version of the response from our bankingp rovider
    /// </summary>
    public class BankResponse
    {
        /// <summary>
        /// A unique identifier for the response
        /// </summary>
        public Guid ResponseId{ get; set; }

        /// <summary>
        /// The unique transaction id
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Success indicator
        /// </summary>
        public bool Success { get; set; }
    }
}
