using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Payment.Gateway.Api.Messaging;
using Payment.Gateway.Api.Services;
using System;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Payment.Gateway.Api.Exceptions;

namespace Payment.Gateway.Api.Controllers
{
    /// <summary>
    /// Manages Payments
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentservice;
        private readonly ILogger<PaymentController> _logger;

        /// <summary>
        /// Payments controller constructor
        /// </summary>
        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            this._paymentservice = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Process a payment through the payment gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentRequest request)
        {
            var response = await this._paymentservice.ProcessPaymentAsync(request);

            return Ok(response);
        }

        /// <summary>
        /// List the payments for a merchant
        /// </summary>
        ///<param name="paymentId">Defaulting for convenience in Swagger</param>
        /// <param name="merchantId">Defaulting for convenience in Swagger</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPaymentDetails(string paymentId = "12345678-9861-4C71-9B9F-201EB65E49D0", string merchantId = "2CB14EAD-9861-4C71-9B9F-201EB65E49D0")
        {
            var paymentGuid = Guid.Parse(paymentId);
            var merchantGuid = Guid.Parse(merchantId);

            try
            {
                var response = await this._paymentservice.GetPaymentDetailsAsync(paymentGuid, merchantGuid);
                return Ok(response);
            }
            catch (PaymentNotFoundException)
            {
                _logger.LogError($"Payment was not found for {paymentGuid} {merchantGuid}");
                return NotFound();
            }
        }
    }
}
