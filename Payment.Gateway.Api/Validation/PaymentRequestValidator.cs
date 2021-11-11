using FluentValidation;
using Payment.Gateway.Api.Messaging;

namespace Payment.Gateway.Api.Validation
{
    /// <summary>
    /// Payment Request Validator
    /// </summary>
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        /// <summary>
        /// PaymentRequestValidator constructor
        /// </summary>
        public PaymentRequestValidator()
        {
            RuleFor(x => x.PaymentId).NotEmpty();

            RuleFor(x => x.MerchantId).NotEmpty();

            RuleFor(x => x.CardNumber).GreaterThan(0);
            RuleFor(x => x.CardNumber.ToString()).Length(16);

            RuleFor(x => x.Amount).GreaterThan(0);
            
            RuleFor(x => x.Cvv).GreaterThan(0);
            RuleFor(x => x.Cvv.ToString()).Length(3);

            RuleFor(x => x.ExpiryMonth).GreaterThan(0);
            RuleFor(x => x.ExpiryMonth).LessThan(13);

            RuleFor(x => x.ExpiryYear).GreaterThan(2000);
            RuleFor(x => x.ExpiryYear).LessThan(3000);
        }
    }
}
