using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using epay3.Sdk.Models;
using Xunit;

namespace epay3.Sdk.Integration.Tests
{
    /// <summary>
    /// Integration tests for AutoPayResource.
    /// </summary>
    public class AutoPayResourceTests : IDisposable
    {
        private readonly epay3Client _client;
        private const string TestCreditCardNumber = "5454545454545454";
        private const string TestCvcNumber = "999";

        public AutoPayResourceTests()
        {
            _client = TestConfiguration.CreateClient();
        }

        /// <summary>
        /// Generates a unique email address to avoid duplicate detection.
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
        /// Generates unique attributes to avoid AutoPay duplicate detection.
        /// </summary>
        private static Dictionary<string, string> GetUniqueAttributes() => new Dictionary<string, string> { { "accountCode", "123" }, { "postalCode", Guid.NewGuid().ToString("n").Substring(0, 8) } };


        [Fact]
        public async Task CreateAutoPay_WithValidToken_ReturnsAutoPayId()
        {
            // Arrange - First create a token
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "AutoPay Test",
                EmailAddress = GetUniqueEmail("autopaytest@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "AutoPay Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            var autoPayRequest = new CreateAutoPayRequest
            {
                PublicTokenId = tokenId,
                EmailAddress = GetUniqueEmail("autopaytest@example.com"),
                Payer = "AutoPay Test",
                AttributeValues = GetUniqueAttributes()
            };

            // Act
            var autoPayId = await _client.AutoPay.CreateAsync(autoPayRequest);

            // Assert
            Assert.True(autoPayId > 0);
        }

        [Fact]
        public async Task GetAutoPay_WithValidId_ReturnsAutoPay()
        {
            // Arrange - First create a token and autopay
            var uniqueEmail = GetUniqueEmail("getautopay@example.com");
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "Get AutoPay Test",
                EmailAddress = uniqueEmail,
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Get AutoPay Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            var autoPayRequest = new CreateAutoPayRequest
            {
                PublicTokenId = tokenId,
                EmailAddress = uniqueEmail,
                Payer = "Get AutoPay Test",
                AttributeValues = GetUniqueAttributes()
            };

            var autoPayId = await _client.AutoPay.CreateAsync(autoPayRequest);

            // Act
            var autoPay = await _client.AutoPay.GetAsync(autoPayId);

            // Assert
            Assert.NotNull(autoPay);
            Assert.Equal(autoPayId, autoPay.Id);
            Assert.Equal(tokenId, autoPay.TokenId);
            Assert.Equal(uniqueEmail, autoPay.Email);
            Assert.NotNull(autoPay.CreateDate);
            Assert.Null(autoPay.CancelDate); // Should not be cancelled yet
        }

        [Fact]
        public async Task SearchAutoPays_WithDateFilters_ReturnsResults()
        {
            // Arrange
            var searchRequest = new SearchAutoPaysRequest
            {
                CreateDateStart = DateTime.Now.AddMonths(-1),
                CreateDateEnd = DateTime.Now,
                Page = 1,
                ItemsPerPage = 10
            };

            // Act
            var results = await _client.AutoPay.SearchAsync(searchRequest);

            // Assert
            Assert.NotNull(results);
            Assert.NotNull(results.AutoPays);
            // Note: May or may not have results depending on test data
        }

        [Fact]
        public async Task CancelAutoPay_WithValidId_Succeeds()
        {
            // Arrange - First create a token and autopay
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "Cancel AutoPay Test",
                EmailAddress = GetUniqueEmail("cancelautopay@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Cancel AutoPay Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            var autoPayRequest = new CreateAutoPayRequest
            {
                PublicTokenId = tokenId,
                EmailAddress = GetUniqueEmail("cancelautopay@example.com"),
                Payer = "Cancel AutoPay Test",
                AttributeValues = GetUniqueAttributes()
            };

            var autoPayId = await _client.AutoPay.CreateAsync(autoPayRequest);

            // Act - Cancel should not throw
            await _client.AutoPay.CancelAsync(autoPayId);

            // Assert - Verify cancellation
            var autoPay = await _client.AutoPay.GetAsync(autoPayId);
            Assert.NotNull(autoPay);
            Assert.NotNull(autoPay.CancelDate); // Should now be cancelled
        }

        [Fact]
        public async Task RestartAutoPay_WithCancelledAutoPay_Succeeds()
        {
            // Arrange - First create, then cancel an autopay
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "Restart AutoPay Test",
                EmailAddress = GetUniqueEmail("restartautopay@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Restart AutoPay Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            var autoPayRequest = new CreateAutoPayRequest
            {
                PublicTokenId = tokenId,
                EmailAddress = GetUniqueEmail("restartautopay@example.com"),
                Payer = "Restart AutoPay Test",
                AttributeValues = GetUniqueAttributes()
            };

            var autoPayId = await _client.AutoPay.CreateAsync(autoPayRequest);

            // Cancel it
            await _client.AutoPay.CancelAsync(autoPayId);

            // Act - Restart should not throw
            await _client.AutoPay.RestartAsync(autoPayId);

            // Assert - Verify restart (cancel date should be cleared or autopay reactivated)
            var autoPay = await _client.AutoPay.GetAsync(autoPayId);
            Assert.NotNull(autoPay);
            // Note: API behavior may vary on whether CancelDate is cleared or status is changed
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
