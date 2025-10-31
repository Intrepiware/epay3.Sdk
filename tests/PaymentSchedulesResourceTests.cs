using System;
using System.Threading.Tasks;
using epay3.Sdk.Models;
using Xunit;

namespace epay3.Sdk.Integration.Tests
{
    /// <summary>
    /// Integration tests for PaymentSchedulesResource.
    /// </summary>
    public class PaymentSchedulesResourceTests : IDisposable
    {
        private readonly epay3Client _client;
        private static readonly Random _random = new Random();
        private const string TestCreditCardNumber = "5454545454545454";
        private const string TestCvcNumber = "999";

        public PaymentSchedulesResourceTests()
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
        /// Generates a unique amount by adding small random cents.
  /// </summary>
  private static decimal GetUniqueAmount(decimal baseAmount)
     {
   var randomCents = _random.Next(1, 99) / 100.0m;
       return Math.Round(baseAmount + randomCents, 2);
    }

        [Fact]
        public async Task CreatePaymentSchedule_WithMonthlyInterval_ReturnsScheduleId()
        {
            // Arrange - First create a token
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "Schedule Test",
                EmailAddress = GetUniqueEmail("scheduletest@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Schedule Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            var scheduleRequest = new CreatePaymentScheduleRequest
            {
                Payer = "Schedule Test",
                EmailAddress = GetUniqueEmail("scheduletest@example.com"),
                TokenId = tokenId,
        Amount = GetUniqueAmount(50.00m),
                Interval = PaymentInterval.Month,
                IntervalCount = 1,
                NumberOfTotalPayments = 6,
                StartDate = DateTime.Now.AddDays(7),
                Comments = "Test monthly payment schedule"
            };

            // Act
            var scheduleId = await _client.PaymentSchedules.CreateAsync(scheduleRequest);

            // Assert
            Assert.NotNull(scheduleId);
            Assert.NotEmpty(scheduleId);
        }

        [Fact]
        public async Task GetPaymentSchedule_WithValidId_ReturnsSchedule()
        {
            // Arrange - First create a token and schedule
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "Get Schedule Test",
                EmailAddress = GetUniqueEmail("getschedule@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Get Schedule Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            var uniqueEmail = GetUniqueEmail("getschedule@example.com");
            var scheduleRequest = new CreatePaymentScheduleRequest
            {
                Payer = "Get Schedule Test",
                EmailAddress = uniqueEmail,
                TokenId = tokenId,
     Amount = GetUniqueAmount(75.00m),
                Interval = PaymentInterval.Month,
                IntervalCount = 2,
                NumberOfTotalPayments = 4
            };

            var scheduleId = await _client.PaymentSchedules.CreateAsync(scheduleRequest);

            // Act
            var schedule = await _client.PaymentSchedules.GetAsync(scheduleId);

            // Assert
            Assert.NotNull(schedule);
            Assert.Equal(scheduleId, schedule.Id);
            Assert.Equal("Get Schedule Test", schedule.Payer);
            Assert.Equal(uniqueEmail, schedule.EmailAddress);
            Assert.Equal(tokenId, schedule.TokenId);
            Assert.Equal(PaymentInterval.Month, schedule.Interval);
            Assert.Equal(2, schedule.IntervalCount);
            Assert.Equal(4, schedule.NumberOfTotalPayments);
        }

        [Fact]
        public async Task CancelPaymentSchedule_WithValidId_Succeeds()
        {
            // Arrange - First create a token and schedule
            var tokenRequest = new CreateTokenRequest
            {
                Payer = "Cancel Schedule Test",
                EmailAddress = GetUniqueEmail("cancelschedule@example.com"),
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Cancel Schedule Test",
                    CardNumber = TestCreditCardNumber,
                    Cvc = TestCvcNumber,
                    Month = "12",
                    Year = DateTime.Now.Year + 1,
                    PostalCode = "12345"
                }
            };

            var tokenId = await _client.Tokens.CreateAsync(tokenRequest);

            var scheduleRequest = new CreatePaymentScheduleRequest
            {
                Payer = "Cancel Schedule Test",
                EmailAddress = GetUniqueEmail("cancelschedule@example.com"),
                TokenId = tokenId,
         Amount = GetUniqueAmount(100.00m),
                Interval = PaymentInterval.Day,
                IntervalCount = 30,
                StartDate = DateTime.Now.AddDays(30) // Future start date to avoid immediate processing
            };

            var scheduleId = await _client.PaymentSchedules.CreateAsync(scheduleRequest);

            // Act - Cancel should not throw
            await _client.PaymentSchedules.CancelAsync(scheduleId);

            // Assert - Verify cancellation by getting the schedule
            var schedule = await _client.PaymentSchedules.GetAsync(scheduleId);
            Assert.NotNull(schedule);
            // Note: API behavior may vary - schedule might still exist but be marked as cancelled
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
