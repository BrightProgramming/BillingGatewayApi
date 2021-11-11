using System.Threading.Tasks;
using Banking.Simulator.Api.Messaging;

namespace Banking.Simulator.Api.Services
{
    /// <summary>
    /// Interface for Payment processing
    /// </summary>
    public interface IPaymentProcessorService
    {
        /// <summary>
        /// Validate payment method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PaymentResponse> ValidatePaymentAsync(PaymentRequest request);
    }
}
