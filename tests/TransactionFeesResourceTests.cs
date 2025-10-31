using System;
using System.Threading.Tasks;
using Xunit;

namespace epay3.Sdk.Integration.Tests
{
    /// <summary>
    /// Integration tests for TransactionFeesResource.
    /// </summary>
    public class TransactionFeesResourceTests : IDisposable
    {
        private readonly epay3Client _client;

        public TransactionFeesResourceTests()
        {
            _client = TestConfiguration.CreateClient();
        }

    [Fact]
        public async Task GetTransactionFees_WithValidAmount_ReturnsFeesCalculation()
        {
   // Arrange
     var amount = 100.00m;

// Act
       var fees = await _client.TransactionFees.GetAsync(amount);

            // Assert
            Assert.NotNull(fees);
            Assert.True(fees.AchPayerFee >= 0, "ACH payer fee should be non-negative");
            Assert.True(fees.CreditCardPayerFee >= 0, "Credit card payer fee should be non-negative");
        }

        [Fact]
        public async Task GetTransactionFees_WithDifferentAmounts_ReturnsProportionalFees()
        {
            // Arrange
  var smallAmount = 50.00m;
   var largeAmount = 500.00m;

       // Act
            var smallFees = await _client.TransactionFees.GetAsync(smallAmount);
            var largeFees = await _client.TransactionFees.GetAsync(largeAmount);

            // Assert
            Assert.NotNull(smallFees);
            Assert.NotNull(largeFees);

            // Larger amounts should generally result in larger fees (or equal for flat fees)
            Assert.True(largeFees.AchPayerFee >= smallFees.AchPayerFee,
                "Large amount ACH fee should be >= small amount ACH fee");
            Assert.True(largeFees.CreditCardPayerFee >= smallFees.CreditCardPayerFee,
                "Large amount credit card fee should be >= small amount credit card fee");
        }

        [Fact]
 public async Task GetTransactionFees_WithZeroAmount_ReturnsValidResponse()
   {
    // Arrange
    var amount = 0.00m;

            // Act
        var fees = await _client.TransactionFees.GetAsync(amount);

            // Assert
            Assert.NotNull(fees);
            Assert.True(fees.AchPayerFee >= 0, "ACH payer fee should be non-negative for zero amount");
            Assert.True(fees.CreditCardPayerFee >= 0, "Credit card payer fee should be non-negative for zero amount");
        }

        [Fact]
        public async Task GetTransactionFees_WithDecimalAmount_ReturnsAccurateFees()
        {
            // Arrange
            var amount = 123.45m;

            // Act
            var fees = await _client.TransactionFees.GetAsync(amount);

            // Assert
            Assert.NotNull(fees);
            Assert.True(fees.AchPayerFee >= 0, "ACH payer fee should be non-negative");
            Assert.True(fees.CreditCardPayerFee >= 0, "Credit card payer fee should be non-negative");
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
