using System.Threading.Tasks;
using FluentAssertions;
using Payment.Gateway.Api.Configuration;
using Payment.Gateway.Api.Services;
using Xunit;

namespace Payment.Gateway.Api.UnitTests.Services
{
    public class UserServiceTests
    {
        [Theory]
        [InlineData("user", "password", true)]
        [InlineData("invalid", "invalid", false)]
        public async Task AuthenticateAsync_WhenGivenCreds_ShouldReturnExpectedResult(string username, string password, bool expectedResult)
        {
            var securityConfig = new SecurityConfig
            {
                ValidUsername = "user",
                ValidPassword = "password",
            };

            var userService = new UserService(securityConfig);
            var response = await userService.AuthenticateAsync(username, password, "Basic");

            response.Succeeded.Should().Be(expectedResult);
        }
    }
}
