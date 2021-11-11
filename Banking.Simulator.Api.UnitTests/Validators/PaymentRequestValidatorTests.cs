using System;
using Banking.Simulator.Api.Validation;
using FluentAssertions;
using Xunit;

namespace Banking.Simulator.Api.UnitTests.Validators
{
    public class PaymentRequestValidatorTests
    {
        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", false)]
        [InlineData("00000000-0000-0000-0000-000000000001", true)]
        public void Validate_ForVariousTransactionIds_ShouldValidateAsExpected(string paymentId, bool expectedResult)
        {
            var paymentRequest = new Messaging.PaymentRequest
            {
                TransactionId = Guid.Parse(paymentId),
                CardNumber = 1234567890123456,
                MerchantId = Guid.NewGuid(),
                Amount = 5,
                Cvv = 111,
                ExpiryMonth = 5,
                ExpiryYear = 2021,
            };

            var validator = new PaymentRequestValidator();
            var validationResult = validator.Validate(paymentRequest);
            validationResult.IsValid.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(1234567890123456, true)]
        [InlineData(123456789012345, false)]
        public void Validate_ForVariousCardNumbers_ShouldValidateAsExpected(long cardNumber, bool expectedResult)
        {
            var paymentRequest = new Messaging.PaymentRequest
            {
                TransactionId = Guid.NewGuid(),
                CardNumber = cardNumber,
                MerchantId = Guid.NewGuid(),
                Amount = 5,
                Cvv = 111,
                ExpiryMonth = 5,
                ExpiryYear = 2021,
            };

            var validator = new PaymentRequestValidator();
            var validationResult = validator.Validate(paymentRequest);
            validationResult.IsValid.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", false)]
        [InlineData("00000000-0000-0000-0000-000000000001", true)]
        public void Validate_ForVariousMerchantIds_ShouldValidateAsExpected(string merchantId, bool expectedResult)
        {
            var paymentRequest = new Messaging.PaymentRequest
            {
                TransactionId = Guid.NewGuid(),
                CardNumber = 1234567890123456,
                MerchantId = Guid.Parse(merchantId),
                Amount = 5,
                Cvv = 111,
                ExpiryMonth = 5,
                ExpiryYear = 2021,
            };

            var validator = new PaymentRequestValidator();
            var validationResult = validator.Validate(paymentRequest);
            validationResult.IsValid.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void Validate_ForVariousAmounts_ShouldValidateAsExpected(int amount, bool expectedResult)
        {
            var paymentRequest = new Messaging.PaymentRequest
            {
                TransactionId = Guid.NewGuid(),
                CardNumber = 1234567890123456,
                MerchantId = Guid.NewGuid(),
                Amount = amount,
                Cvv = 111,
                ExpiryMonth = 5,
                ExpiryYear = 2021,
            };

            var validator = new PaymentRequestValidator();
            var validationResult = validator.Validate(paymentRequest);
            validationResult.IsValid.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(123, true)]
        [InlineData(12, false)]
        [InlineData(1, false)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        [InlineData(-12, false)]
        [InlineData(-123, false)]
        public void Validate_ForVariousCvvs_ShouldValidateAsExpected(int cvv, bool expectedResult)
        {
            var paymentRequest = new Messaging.PaymentRequest
            {
                TransactionId = Guid.NewGuid(),
                CardNumber = 1234567890123456,
                MerchantId = Guid.NewGuid(),
                Amount = 10,
                Cvv = cvv,
                ExpiryMonth = 5,
                ExpiryYear = 2021,
            };

            var validator = new PaymentRequestValidator();
            var validationResult = validator.Validate(paymentRequest);
            validationResult.IsValid.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(13, false)]
        [InlineData(12, true)]
        [InlineData(11, true)]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void Validate_ForVariousExpiryMonth_ShouldValidateAsExpected(int expiryMonth, bool expectedResult)
        {
            var paymentRequest = new Messaging.PaymentRequest
            {
                TransactionId = Guid.NewGuid(),
                CardNumber = 1234567890123456,
                MerchantId = Guid.NewGuid(),
                Amount = 10,
                Cvv = 123,
                ExpiryMonth = expiryMonth,
                ExpiryYear = 2021,
            };

            var validator = new PaymentRequestValidator();
            var validationResult = validator.Validate(paymentRequest);
            validationResult.IsValid.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(3001, false)]
        [InlineData(3000, false)]
        [InlineData(2999, true)]
        [InlineData(2001, true)]
        [InlineData(2000, false)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void Validate_ForVariousExpiryYear_ShouldValidateAsExpected(int expiryYear, bool expectedResult)
        {
            var paymentRequest = new Messaging.PaymentRequest
            {
                TransactionId = Guid.NewGuid(),
                CardNumber = 1234567890123456,
                MerchantId = Guid.NewGuid(),
                Amount = 10,
                Cvv = 123,
                ExpiryMonth = 12,
                ExpiryYear = expiryYear,
            };

            var validator = new PaymentRequestValidator();
            var validationResult = validator.Validate(paymentRequest);
            validationResult.IsValid.Should().Be(expectedResult);
        }
    }
}
