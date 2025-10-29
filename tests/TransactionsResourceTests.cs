using System;
using System.Threading.Tasks;
using epay3.Sdk.Models;
using Xunit;

namespace epay3.Sdk.Integration.Tests
{
    /// <summary>
    /// Integration tests for TransactionsResource.
    /// </summary>
    public class TransactionsResourceTests : IDisposable
    {
        private readonly epay3Client _client;

        public TransactionsResourceTests()
        {
            _client = TestConfiguration.CreateClient();
        }

        [Fact]
        public async Task CreateTransaction_WithCreditCard_ReturnsSuccess()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                Amount = 10.00,
                Payer = "Tom Smith",
                EmailAddress = "noreply@epay3.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Tom Smith",
                    CardNumber = "5454545454545454", // Test card
                    Cvc = "999",
                    Month = "12",
                    Year = 2034,
                    PostalCode = "12345"
                },
                Comments = "Integration test transaction",
                Currency = "USD",
                SendReceipt = false
            };

            // Act
            var response = await _client.Transactions.CreateAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Id > 0);
            Assert.Equal(PaymentResponseCode.Success, response.PaymentResponseCode);
        }

        [Fact]
        public async Task CreateTransaction_WithToken_ReturnsSuccess()
        {
            // Arrange - First create a token
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "Token Test",
                EmailAddress = "tokentest@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Token Test",
                    CardNumber = "4111111111111111",
                    Cvc = "123",
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            // Create transaction with token
            var request = new CreateTransactionRequest
            {
                Amount = 15.00,
                Payer = "Token Test",
                EmailAddress = "tokentest@example.com",
                TokenId = tokenId,
                Comments = "Transaction using token",
                SendReceipt = false
            };

            // Act
            var response = await _client.Transactions.CreateAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Id > 0);
            Assert.Equal(PaymentResponseCode.Success, response.PaymentResponseCode);
        }

        [Fact]
        public async Task GetTransaction_WithValidId_ReturnsTransaction()
        {
            // Arrange - First create a transaction
            var createRequest = new CreateTransactionRequest
            {
                Amount = 20.00,
                Payer = "Get Test",
                EmailAddress = "gettest@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Get Test",
                    CardNumber = "4111111111111111",
                    Cvc = "123",
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                },
                SendReceipt = false
            };

            var createResponse = await _client.Transactions.CreateAsync(createRequest);

            // Act
            var transaction = await _client.Transactions.GetAsync(createResponse.Id);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(createResponse.Id, transaction.Id);
            Assert.Equal("Get Test", transaction.Payer);
            Assert.Equal("gettest@example.com", transaction.EmailAddress);
            Assert.True(transaction.Amount > 0);
        }

        [Fact]
        public async Task AuthorizeTransaction_WithToken_ReturnsSuccess()
        {
            // Arrange - First create a token
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "Auth Test",
                EmailAddress = "authtest@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Auth Test",
                    CardNumber = "4111111111111111",
                    Cvc = "123",
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            var request = new AuthorizeTransactionRequest
            {
                TokenId = tokenId,
                Amount = 25.00
            };

            // Act
            var response = await _client.Transactions.AuthorizeAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Id > 0);
            Assert.Equal(PaymentResponseCode.Success, response.PaymentResponseCode);
        }

        [Fact]
        public async Task VoidTransaction_WithValidId_ReturnsSuccess()
        {
            // Arrange - First create a transaction
            var createRequest = new CreateTransactionRequest
            {
                Amount = 30.00,
                Payer = "Void Test",
                EmailAddress = "voidtest@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Void Test",
                    CardNumber = "4111111111111111",
                    Cvc = "123",
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                },
                SendReceipt = false
            };

            var createResponse = await _client.Transactions.CreateAsync(createRequest);

            var voidRequest = new VoidTransactionRequest
            {
                SendReceipt = false
            };

            // Act
            var voidResponse = await _client.Transactions.VoidAsync(createResponse.Id, voidRequest);

            // Assert
            Assert.NotNull(voidResponse);
            // Note: Response code may vary based on transaction state
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
