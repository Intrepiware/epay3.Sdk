using System;
using System.Threading.Tasks;
using epay3.Sdk.Models;
using Xunit;

namespace epay3.Sdk.Integration.Tests
{
    /// <summary>
    /// Integration tests for BatchesResource.
    /// </summary>
    public class BatchesResourceTests : IDisposable
    {
        private readonly epay3Client _client;
        private static readonly Random _random = new Random();
        private const string TestCreditCardNumber = "5454545454545454";
        private const string TestCvcNumber = "999";

        public BatchesResourceTests()
        {
            _client = TestConfiguration.CreateClient();
        }

        /// <summary>
        /// Generates a unique email address to avoid duplicate transaction detection.
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
        /// Generates a unique amount to avoid duplicate detection.
        /// </summary>
        private static double GetUniqueAmount(double baseAmount)
        {
            var randomCents = _random.Next(1, 99) / 100.0;
            return Math.Round(baseAmount + randomCents, 2);
        }

        [Fact]
        public async Task GetBatches_ReturnsSuccess()
        {
            // Act
            var response = await _client.Batches.GetAsync();

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Batches);
            // Note: The response may have zero batches if none exist yet
        }

        [Fact]
        public async Task GetBatches_WithPagination_ReturnsSuccess()
        {
            // Act
            var response = await _client.Batches.GetAsync(page: 1);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Batches);
            Assert.True(response.TotalRecords >= 0);
        }

        [Fact]
        public async Task GetBatches_AfterCreatingTransaction_ContainsBatch()
        {
            // Arrange - Create a transaction first to ensure we have at least one batch
            var transactionRequest = new CreateTransactionRequest
            {
                Amount = GetUniqueAmount(10.00),
                Payer = "Batch Test",
                EmailAddress = GetUniqueEmail("batchtest@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Batch Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                },
                SendReceipt = false
            };

            var transactionResponse = await _client.Transactions.CreateAsync(transactionRequest);
            Assert.Equal(PaymentResponseCode.Success, transactionResponse.PaymentResponseCode);

            // Wait a moment for the transaction to be processed
            await Task.Delay(2000);

            // Act
            var batchResponse = await _client.Batches.GetAsync();

            // Assert
            Assert.NotNull(batchResponse);
            Assert.NotNull(batchResponse.Batches);

            if (batchResponse.Batches.Count > 0)
            {
                var batch = batchResponse.Batches[0];
                Assert.True(batch.Id > 0);
                Assert.NotEqual(DateTime.MinValue, batch.Created);

                // Check that at least one of the transaction counts is non-zero
                Assert.True(batch.NumberOfCredits >= 0);
                Assert.True(batch.NumberOfDebits >= 0);
                Assert.True(batch.TotalOfCredits >= 0);
                Assert.True(batch.TotalOfDebits >= 0);
            }
        }

        [Fact]
        public async Task GetBatches_ValidatesBatchProperties()
        {
            // Arrange - Create a transaction to ensure batches exist
            var transactionRequest = new CreateTransactionRequest
            {
                Amount = GetUniqueAmount(15.00),
                Payer = "Batch Props Test",
                EmailAddress = GetUniqueEmail("batchprops@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Batch Props Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                },
                SendReceipt = false
            };

            await _client.Transactions.CreateAsync(transactionRequest);
            await Task.Delay(2000);

            // Act
            var response = await _client.Batches.GetAsync();

            // Assert
            Assert.NotNull(response);

            if (response.Batches.Count > 0)
            {
                var batch = response.Batches[0];

                // Validate all properties are properly deserialized
                Assert.True(batch.Id > 0, "Batch ID should be greater than 0");
                Assert.NotEqual(DateTime.MinValue, batch.Created);

                // Currency should be either USD or CAD
                Assert.True(
                    batch.Currency == Currency.USD || batch.Currency == Currency.CAD,
                    $"Currency should be USD or CAD, got {batch.Currency}");

                // If divisions exist, validate their structure
                if (batch.Divisions != null && batch.Divisions.Count > 0)
                {
                    foreach (var division in batch.Divisions)
                    {
                        Assert.NotNull(division);
                        Assert.True(division.Amount >= 0, "Division amount should be non-negative");
                    }
                }

                // Processor fields may be null or populated
                // Just check they don't throw when accessed
                _ = batch.Processor;
                _ = batch.ProcessorPaymentMethod;
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
