using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Payment.Gateway.Api.Messaging;

namespace Payment.Gateway.Api.Services
{
    /// <summary>
    /// Use the bank service to validate the payment
    /// </summary>
    public class BankServiceClient : IBankServiceClient
    {
        private const string ValidateEndPoint = "api/Payment";

        private readonly HttpClient _httpClient;
        private readonly ILogger<BankServiceClient> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public BankServiceClient(HttpClient httpClient, ILogger<BankServiceClient> logger)
        {
            this._httpClient = httpClient;
            this._logger = logger;
        }

        /// <summary>
        /// Use the bank service to validate the payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BankResponse> ProcessPaymentAsync(BankRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            try
            {
                // Possibly use Polly to perform retries
                response = await this._httpClient.PostAsync(ValidateEndPoint, data);
            }
            catch (Exception ex)
            {
                // Potentially use Polly or equivalent to allow retries.
                this._logger.LogError($"Failed to call banking service api - error {ex.Message}");
                
                // rethrow to allow the global exception handler to catch
                throw;
            }

            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<BankResponse>(content);
                }

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new HttpRequestException();
                }

                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error calling bank service endpoint:POST api/payment Error: {response.StatusCode} {response.ReasonPhrase} {error}");
            }

            throw new HttpRequestException();
        }
    }
}
