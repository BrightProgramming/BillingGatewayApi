using System.Threading.Tasks;
using Banking.Simulator.Api.Services;
using FluentAssertions;
using Xunit;

namespace Banking.Simulator.Api.UnitTests.Services
{
    public class UserServiceTests
    {
        [Theory]
        [InlineData("bank", "bank", true)]
        [InlineData("invalid", "invalid", false)]
        public async Task AuthenticateAsync_WhenGivenCreds_ShouldReturnExpectedResult(string username, string password, bool expectedResult)
        {
            var userService = new UserService();
            var response = await userService.AuthenticateAsync(username, password, "Basic");

            response.Succeeded.Should().Be(expectedResult);
        }
    }
}
