using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Banking.Simulator.Api.Services
{
    /// <summary>
    /// Implementation for user service
    /// </summary>
    public class UserService : IUserService
    {
        private const string ValidUser = "bank";
        private const string ValidPassword = "bank";

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="schemeName"></param>
        /// <returns></returns>
        public async Task<AuthenticateResult> AuthenticateAsync(string userName, string password, string schemeName)
        {
            await Task.Delay(1); // Make a database call or similar to validate

            if (userName == ValidUser && password == ValidPassword)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "user"),
                    new Claim(ClaimTypes.Name, "user"),
                };
                var identity = new ClaimsIdentity(claims, schemeName);
                var principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, schemeName);
                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Invalid Username or Password");
        }
    }
}
