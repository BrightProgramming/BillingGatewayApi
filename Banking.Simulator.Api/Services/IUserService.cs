using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Banking.Simulator.Api.Services
{
    /// <summary>
    /// Interface for user service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Authenticate user method.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="schemeName"></param>
        /// <returns></returns>
        Task<AuthenticateResult> AuthenticateAsync(string userName, string password, string schemeName);
    }
}
