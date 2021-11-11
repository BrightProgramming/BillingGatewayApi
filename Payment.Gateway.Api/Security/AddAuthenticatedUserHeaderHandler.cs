using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Payment.Gateway.Api.Configuration;

namespace Payment.Gateway.Api.Security
{
    /// <summary>
    /// Adds an authentication header for http calls
    /// </summary>
    public class AddAuthenticatedUserHeaderHandler : DelegatingHandler
    {
        private readonly BankServiceConfig _bankServiceConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddAuthenticatedUserHeaderHandler(BankServiceConfig bankServiceConfig)
        {
            _bankServiceConfig = bankServiceConfig;
        }
        
        /// <summary>
        /// Delegating handler for sending a message with security header
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var base64Header = Encoding.ASCII.GetBytes($"{this._bankServiceConfig.Username}:{this._bankServiceConfig.Password}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(base64Header));

            return base.SendAsync(request, cancellationToken);
        }
    }
}
