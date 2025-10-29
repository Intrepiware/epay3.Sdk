using System;
using System.Threading.Tasks;
using epay3.Sdk.Models;
using Xunit;

namespace epay3.Sdk.Integration.Tests
{
    /// <summary>
    /// Integration tests for TokensResource.
    /// </summary>
    public class TokensResourceTests : IDisposable
    {
        private readonly epay3Client _client;

        public TokensResourceTests()
        {
            _client = TestConfiguration.CreateClient();
        }

        [Fact]
        public async Task CreateToken_WithCreditCard_ReturnsTokenId()
        {
            // Arrange
            var request = new CreateTokenRequest
            {
                Payer = "John Doe",
                EmailAddress = "john.doe@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "John Doe",
                    CardNumber = "4111111111111111", // Test card number
                    Cvc = "123",
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            // Act
            var tokenId = await _client.Tokens.CreateAsync(request);

            // Assert
            Assert.NotNull(tokenId);
            Assert.NotEmpty(tokenId);
        }

        [Fact]
        public async Task CreateToken_WithBankAccount_ReturnsTokenId()
        {
            // Arrange
            var request = new CreateTokenRequest
            {
                Payer = "Jane Smith",
                EmailAddress = "jane.smith@example.com",
                BankAccountInformation = new BankAccountInformation
                {
                    AccountHolder = "Jane Smith",
                    FirstName = "Jane",
                    LastName = "Smith",
                    AccountType = BankAccountType.PersonalChecking,
                    RoutingNumber = "021000021", // Test routing number
                    AccountNumber = "123456789"
                }
            };

            // Act
            var tokenId = await _client.Tokens.CreateAsync(request);

            // Assert
            Assert.NotNull(tokenId);
            Assert.NotEmpty(tokenId);
        }

        [Fact]
        public async Task GetToken_WithValidId_ReturnsToken()
        {
            // Arrange - First create a token
            var createRequest = new CreateTokenRequest
            {
                Payer = "Test User",
                EmailAddress = "test@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Test User",
                    CardNumber = "4111111111111111",
                    Cvc = "123",
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(createRequest);

            // Act
            var token = await _client.Tokens.GetAsync(tokenId);

            // Assert
            Assert.NotNull(token);
            Assert.Equal(tokenId, token.Id);
            Assert.Equal("Test User", token.Payer);
            Assert.Equal("test@example.com", token.EmailAddress);
            Assert.NotNull(token.MaskedAccountNumber);
        }

        [Fact]
        public async Task DeleteToken_WithValidId_Succeeds()
        {
            // Arrange - First create a token
            var createRequest = new CreateTokenRequest
            {
                Payer = "Delete Test",
                EmailAddress = "delete@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Delete Test",
                    CardNumber = "4111111111111111",
                    Cvc = "123",
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(createRequest);

            // Act - Delete should not throw
            await _client.Tokens.DeleteAsync(tokenId);

            // Assert - Trying to get the deleted token should fail
            await Assert.ThrowsAsync<epay3.Sdk.Exceptions.epay3ApiException>(
                async () => await _client.Tokens.GetAsync(tokenId));
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
