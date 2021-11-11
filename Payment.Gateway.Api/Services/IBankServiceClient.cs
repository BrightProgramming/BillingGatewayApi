using System.Threading.Tasks;
using Payment.Gateway.Api.Messaging;

namespace Payment.Gateway.Api.Services
{
    /// <summary>
    /// Interface for calling the bank service implementation
    /// </summary>
    public interface IBankServiceClient
    {
        /// <summary>
        /// Call the Bank Service to process the payment
        /// </summary>
        Task<BankResponse> ProcessPaymentAsync(BankRequest request);
    }
}
