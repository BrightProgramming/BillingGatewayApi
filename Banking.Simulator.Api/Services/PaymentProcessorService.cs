using System;
using System.Threading.Tasks;
using Banking.Simulator.Api.Messaging;

namespace Banking.Simulator.Api.Services
{
    /// <summary>
    /// Implementation for payment processor
    /// </summary>
    public class PaymentProcessorService : IPaymentProcessorService
    {
        /// <summary>
        /// Validate payment method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PaymentResponse> ValidatePaymentAsync(PaymentRequest request)
        {
            var success = request.CardNumber % 2 == 0;

            var response = new PaymentResponse
            {
                ResponseId = Guid.NewGuid(),
                TransactionId = request.TransactionId,
                Success = success,
            };

            return await Task.FromResult(response);
        }
    }
}
