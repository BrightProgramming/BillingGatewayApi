using System;
using System.Net;
using Moq;
using Payment.Gateway.Api.Services;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq.Protected;
using Xunit;

namespace Payment.Gateway.Api.UnitTests.Services
{
    public class BankServiceClientTests
    {
        [Fact]
        public async Task ProcessPaymentAsync_WhenExecuted_ShouldUseHttpClient()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var mockedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""transactionId"": ""12345678-9861-4C71-9B9F-201EB65E49D0"", ""success"": true}"),
            };

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockedResponse);

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost"),
            };

            var bankServiceClient = new BankServiceClient(httpClient, new NullLogger<BankServiceClient>());

            var request = new Messaging.BankRequest
            {
                TransactionId = Guid.NewGuid(),
            };

            var response = await bankServiceClient.ProcessPaymentAsync(request);

            response.TransactionId.Should().Be(Guid.Parse("12345678-9861-4C71-9B9F-201EB65E49D0"));
            response.Success.Should().BeTrue();
            handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post), ItExpr.IsAny<CancellationToken>());
        }
    }
}
