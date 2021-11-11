using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Banking.Simulator.Api.Messaging;
using Banking.Simulator.Api.Services;
using Microsoft.AspNetCore.Http;

namespace Banking.Simulator.Api.Controllers
{
    /// <summary>
    /// Simulates a bank processor
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentProcessorService _paymentProcessorService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="paymentProcessorService"></param>
        public PaymentController(IPaymentProcessorService paymentProcessorService)
        {
            this._paymentProcessorService = paymentProcessorService;
        }

        /// <summary>
        /// Process a payment through the payment gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentRequest request)
        {
            var response = await this._paymentProcessorService.ValidatePaymentAsync(request);

            return Ok(response);
        }
    }
}
