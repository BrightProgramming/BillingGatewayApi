using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Payment.Gateway.Api.Messaging;
using Payment.Gateway.Api.Repository;

namespace Payment.Gateway.Api.Services
{
    /// <summary>
    /// Service for managing payments
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankServiceClient _bankService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentService(IPaymentRepository paymentRepository, IBankServiceClient bankService, IMapper mapper, ILogger<PaymentService> logger)
        {
            this._paymentRepository = paymentRepository;
            this._bankService = bankService;
            this._mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Process  payment request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            // Save the request with a status of pending
            var mongoPaymentRequest = this._mapper.Map<Repository.Models.PaymentRequest>(request);
            var insertedPayment = await this._paymentRepository.InsertPaymentRequestAsync(mongoPaymentRequest);

            // Call the banking service to perform the operation
            var bankRequest = this._mapper.Map<BankRequest>(mongoPaymentRequest);
            bankRequest.TransactionId = insertedPayment.TransactionId;
            var bankResponse = await this._bankService.ProcessPaymentAsync(bankRequest);

            _logger.LogInformation($"The bank service returned a value of {bankResponse.Success} for transactionId {bankResponse.TransactionId}");

            // Update the status of the request to indicate success
            var updateResponse = await this._paymentRepository.UpdatePaymentRequestAsync(insertedPayment.TransactionId, bankResponse.Success);
            var mappedUpdateResponse = this._mapper.Map<PaymentResponse>(updateResponse);
            return mappedUpdateResponse;
        }

        /// <summary>
        /// Gets details for a particular payment
        /// </summary>
        public async Task<GetPaymentDetailsResponse> GetPaymentDetailsAsync(Guid paymentId, Guid merchantId)
        {
            var mongoPayment = await this._paymentRepository.GetPaymentDetailsAsync(paymentId, merchantId);
            var mappedPayment = this._mapper.Map<GetPaymentDetailsResponse>(mongoPayment);
            
            return mappedPayment;
        }
    }
}
