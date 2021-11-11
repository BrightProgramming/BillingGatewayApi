using System;

namespace Payment.Gateway.Api.Extensions
{
    /// <summary>
    /// Provides string extension methods
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Mask part or all of a string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="maskCharacter"></param>
        /// <param name="numberOfCharactersToRetain"></param>
        /// <returns></returns>
        public static string Mask(this string source, char maskCharacter, int numberOfCharactersToRetain)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            if (numberOfCharactersToRetain > source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(source), "Retain length is greater than string length");
            }

            var maskedString = source.Substring(0, numberOfCharactersToRetain)
                .PadRight(source.Length, maskCharacter);

            return maskedString;
        }
    }
}
