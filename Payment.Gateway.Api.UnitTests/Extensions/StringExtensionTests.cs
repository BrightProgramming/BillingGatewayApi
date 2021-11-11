using System;
using FluentAssertions;
using Payment.Gateway.Api.Extensions;
using Xunit;

namespace Payment.Gateway.Api.UnitTests.Extensions
{
    public class StringExtensionTests
    {
        [Fact]
        public void Mask_WhenGivenValidValue_ShouldReturnMaskedValue()
        {
            var source = "0123456789123456";
            var masked = source.Mask('*', 8);

            masked.Should().Be("01234567********");
        }

        [Fact]
        public void Mask_WhenGivenNull_ShouldThrowArgumentOutOfRange()
        {
            string source = null;
            Action act = () => source.Mask('*', 8);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Mask_WhenGivenEmpty_ShouldThrowArgumentOutOfRange()
        {
            string source = string.Empty;
            Action act = () => source.Mask('*', 8);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Mask_WhenGivenInvalidLengthToRetain_ShouldThrowArgumentOutOfRange()
        {
            var source = "hello";
            Action act = () => source.Mask('*', 8);
            act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Retain length is greater than string length (Parameter 'source')");
        }

        [Fact]
        public void Mask_WhenGivenZeroToRetain_ShouldMaskEntireString()
        {
            var source = "hello";
            var masked = source.Mask('*', 0);
            masked.Should().Be("*****");
        }
    }
}
