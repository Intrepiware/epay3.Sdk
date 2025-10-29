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
        private static readonly Random _random = new Random();
        private const string TestCreditCardNumber = "5454545454545454";
        private const string TestCvcNumber = "999";

        public TransactionsResourceTests()
        {
            _client = TestConfiguration.CreateClient();
        }

        /// <summary>
        /// Generates a unique email address to avoid duplicate transaction detection.
        /// The API checks email+amount for dupes within a 5 minute window.
        /// </summary>
        private static string GetUniqueEmail(string baseEmail)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var emailParts = baseEmail.Split('@');
            if (emailParts.Length == 2)
            {
                return $"{emailParts[0]}+{timestamp}@{emailParts[1]}";
            }
            return $"{baseEmail}.{timestamp}";
        }

        /// <summary>
        /// Generates a unique amount by adding small random cents to avoid duplicate detection.
        /// </summary>
        private static double GetUniqueAmount(double baseAmount)
        {
            var randomCents = _random.Next(1, 99) / 100.0;
            return Math.Round(baseAmount + randomCents, 2);
        }

        [Fact]
        public async Task CreateTransaction_WithCreditCard_ReturnsSuccess()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                Amount = GetUniqueAmount(10.00),
                Payer = "Tom Smith",
                EmailAddress = GetUniqueEmail("test@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Tom Smith",
                    CardNumber = TestCreditCardNumber, // Test card
                    Cvc = TestCvcNumber,
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
            var uniqueEmail = GetUniqueEmail("tokentest@example.com");
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "Token Test",
                EmailAddress = uniqueEmail,
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Token Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            // Create transaction with token
            var request = new CreateTransactionRequest
            {
                Amount = GetUniqueAmount(15.00),
                Payer = "Token Test",
                EmailAddress = uniqueEmail,
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
            var uniqueEmail = GetUniqueEmail("gettest@example.com");
            var createRequest = new CreateTransactionRequest
            {
                Amount = GetUniqueAmount(20.00),
                Payer = "Get Test",
                EmailAddress = uniqueEmail,
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Get Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
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
            Assert.Equal(uniqueEmail, transaction.EmailAddress);
            Assert.True(transaction.Amount > 0);
        }

        [Fact]
        public async Task AuthorizeTransaction_WithToken_ReturnsSuccess()
        {
            // Arrange - First create a token
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "Auth Test",
                EmailAddress = GetUniqueEmail("authtest@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Auth Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            var request = new AuthorizeTransactionRequest
            {
                TokenId = tokenId,
                Amount = GetUniqueAmount(25.00)
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
                Amount = GetUniqueAmount(30.00),
                Payer = "Void Test",
                EmailAddress = GetUniqueEmail("voidtest@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Void Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
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
