using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Payment.Gateway.Api.Configuration;

namespace Payment.Gateway.Api.Services
{
    /// <summary>
    /// Service implementation for User
    /// </summary>
    public class UserService : IUserService
    {
        private readonly SecurityConfig _securityConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserService(SecurityConfig securityConfig)
        {
            this._securityConfig = securityConfig;
        }

        /// <summary>
        /// Perform an authentication
        /// </summary>
        public async Task<AuthenticateResult> AuthenticateAsync(string userName, string password, string schemeName)
        {
            await Task.Delay(1); // Make a database call or similar to validate

            if (userName == this._securityConfig.ValidUsername && password == this._securityConfig.ValidPassword)
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
