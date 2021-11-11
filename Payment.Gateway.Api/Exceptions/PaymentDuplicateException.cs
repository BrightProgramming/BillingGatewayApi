using System;

namespace Payment.Gateway.Api.Exceptions
{
    /// <summary>
    /// Payment was a duplicate exception
    /// </summary>
    public class PaymentDuplicateException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentDuplicateException()
        {
        }

        /// <summary>
        /// Constructor for inner exception
        /// </summary>
        public PaymentDuplicateException(string message, Exception ex) : base(message, ex)
        {
            
        }
    }
}
