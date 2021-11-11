using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Payment.Gateway.Api.Services
{
    /// <summary>
    /// Interface for user service functionality
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Perform an authentication
        /// </summary>
        /// <returns></returns>
        Task<AuthenticateResult> AuthenticateAsync(string userName, string password, string schemeName);
    }
}
