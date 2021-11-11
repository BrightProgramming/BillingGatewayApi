using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Payment.Gateway.Api.TestHarness.Models;

namespace Payment.Gateway.Api.TestHarness
{
    class Program
    {
        private const string PaymentApiUserName = "user";
        private const string PaymentApiPassword = "password";
        static async Task Main(string[] args)
        {
            string userChoice;

            do
            {
                Console.Clear();
                Console.WriteLine("Payment Gateway Api Menu");
                Console.WriteLine("1 - Raise a payment Request");
                Console.WriteLine("2 - Get details for a payment Request");
                Console.WriteLine("q - Quit");
                Console.WriteLine("");

                userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        await RaisePaymentRequestAsync();
                        break;
                    case "2":
                        await GetPaymentDetailsAsync();
                        break;
                }
            } 
            while (userChoice != "q");
        }

        private static async Task RaisePaymentRequestAsync()
        {
            Console.Clear();
            Console.WriteLine("--------------- Raise a payment Request ---------------");
            Console.WriteLine("");
            Console.WriteLine("Press enter for default");
            Console.WriteLine("");

            Console.Write("PaymentId: (12345678-9861-4C71-9B9F-201EB65E49D0) : ");
            var paymentId = "12345678-9861-4C71-9B9F-201EB65E49D0";
            var temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                paymentId = temp;
            }

            Console.Write("MerchantId: (2CB14EAD-9861-4C71-9B9F-201EB65E49D0) : ");
            var merchantId = "2CB14EAD-9861-4C71-9B9F-201EB65E49D0";
            temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                merchantId = temp;
            }

            Console.Write("Card Number: (1234123412341234) : ");
            var cardNumber = "1234123412341234";
            temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                cardNumber = temp;
            }

            Console.Write("ExpiryMonth: (12) : ");
            var expiryMonth = "12";
            temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                expiryMonth = temp;
            }

            Console.Write("ExpiryYear: (2023) : ");
            var expiryYear = "2023";
            temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                expiryYear = temp;
            }

            Console.Write("Amount: (10.15) : ");
            var amount = "10.15";
            temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                amount = temp;
            }

            Console.Write("Cvv: (123) : ");
            var cvv = "123";
            temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                cvv = temp;
            }

            Console.Write("TransactionDate: (2021-10-29T12:05:08.3587598+00:00) : ");
            var transactionDate = "2021-10-29T12:05:08.3587598+00:00";
            temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                transactionDate = temp;
            }

            var request = new PaymentRequest
            {
                PaymentId = Guid.Parse(paymentId),
                MerchantId = Guid.Parse(merchantId),
                CardNumber = long.Parse(cardNumber),
                ExpiryMonth = int.Parse(expiryMonth),
                ExpiryYear = int.Parse(expiryYear),
                Amount = decimal.Parse(amount),
                Cvv = int.Parse(cvv),
                TransactionDate = DateTimeOffset.Parse(transactionDate),
            };

            Console.WriteLine("Press enter to send");
            Console.ReadLine();

            var json = JsonConvert.SerializeObject(request);

            try
            {
                using (var httpClient = GetHttpClient())
                {
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("api/Payment", data);

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Got response {content}");
                        }
                        else
                        {
                            var error = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"ERROR calling payment api endpoint - Error: {response.StatusCode} {response.ReasonPhrase} {error}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to call the banking gateway api - error {ex.Message}");
                Console.WriteLine("Are you sure the service is running");
            }

            Console.WriteLine("Press Enter");
            Console.ReadLine();
        }

        private static async Task GetPaymentDetailsAsync()
        {
            Console.Clear();
            Console.WriteLine("--------------- Get details for a payment ---------------");
            Console.WriteLine("");
            Console.WriteLine("Press enter for default");
            Console.WriteLine("");

            Console.Write("PaymentId: (12345678-9861-4C71-9B9F-201EB65E49D0) : ");
            var paymentId = "12345678-9861-4C71-9B9F-201EB65E49D0";
            var temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                paymentId = temp;
            }

            Console.Write("MerchantId: (2CB14EAD-9861-4C71-9B9F-201EB65E49D0) : ");
            var merchantId = "2CB14EAD-9861-4C71-9B9F-201EB65E49D0";
            temp = Console.ReadLine();
            if (!string.IsNullOrEmpty(temp))
            {
                merchantId = temp;
            }

            Console.WriteLine("Press enter to send");
            Console.ReadLine();

            try
            {
                using (var httpClient = GetHttpClient())
                {
                    var url = $"api/Payment?paymentId={paymentId}&merchantId={merchantId}";
                    ; var response = await httpClient.GetAsync(url);

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();

                            // This just cleans up the format a bit
                            var deserialised = JsonConvert.DeserializeObject<GetPaymentDetailsResponse>(content);
                            var serialised = JsonConvert.SerializeObject(deserialised, Formatting.Indented);

                            Console.WriteLine($"Got response {serialised}");
                        }
                        else
                        {
                            var error = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"ERROR calling payment api endpoint - Error: {response.StatusCode} {response.ReasonPhrase} {error}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to call the banking gateway api - error {ex.Message}");
                Console.WriteLine("Are you sure the service is running");
            }
            
            Console.WriteLine("Press Enter");
            Console.ReadLine();
        }

        private static HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            var client = new HttpClient(handler);
            
            client.BaseAddress = new Uri("https://localhost:5001/");

            var base64Header = Encoding.ASCII.GetBytes($"{PaymentApiUserName}:{PaymentApiPassword}");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(base64Header));

            return client;
        }
    }
}
