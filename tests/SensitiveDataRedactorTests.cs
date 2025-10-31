using System;
using epay3.Sdk.Attributes;
using epay3.Sdk.Http;
using epay3.Sdk.Models;
using Newtonsoft.Json;
using Xunit;

namespace epay3.Sdk.Integration.Tests
{
    /// <summary>
    /// Unit tests for SensitiveDataRedactor to ensure proper redaction of payment information.
    /// </summary>
    public class SensitiveDataRedactorTests
    {
        // Test model with sensitive properties
        private class TestPaymentModel
        {
            public string Payer { get; set; }
            public string EmailAddress { get; set; }

            [SensitiveData(RedactionMode.MaskShowFirstAndLast4)]
            public string CardNumber { get; set; }

            [SensitiveData(RedactionMode.Complete, RedactedValue = "XXX")]
            public string Cvc { get; set; }

            [SensitiveData(RedactionMode.MaskShowFirstAndLast4)]
            public string AccountNumber { get; set; }

            [SensitiveData(RedactionMode.Complete, RedactedValue = "XXXXXXXXX")]
            public string RoutingNumber { get; set; }
        }

        [Fact]
        public void RedactSensitiveData_WithCreditCardInformation_RedactsCardNumber()
        {
            // Arrange
            var model = new CreateTokenRequest
            {
                Payer = "John Doe",
                EmailAddress = "john@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "John Doe",
                    CardNumber = "4111111111111111",
                    Cvc = "123",
                    Month = "12",
                    Year = 2025
                }
            };
            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(json);

            // Assert - MaskShowFirstAndLast4: 4111 (first 4) + XXXXXXXX (8 middle digits) + 1111 (last 4)
            Assert.Contains("XXXXXXXXXXXX1111", redacted);
            Assert.DoesNotContain("4111111111111111", redacted);
        }

        [Fact]
        public void RedactSensitiveData_WithCreditCardInformation_CompletelyRedactsCvc()
        {
            // Arrange
            var model = new CreateTokenRequest
            {
                Payer = "John Doe",
                EmailAddress = "john@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "John Doe",
                    CardNumber = "4111111111111111",
                    Cvc = "123",
                    Month = "12",
                    Year = 2025
                }
            };
            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(json);

            // Assert
            Assert.Contains("\"XXX\"", redacted);
            Assert.DoesNotContain("\"123\"", redacted);
        }

        [Fact]
        public void RedactSensitiveData_WithBankAccountInformation_RedactsAccountNumber()
        {
            // Arrange
            var model = new CreateTokenRequest
            {
                Payer = "Jane Smith",
                EmailAddress = "jane@example.com",
                BankAccountInformation = new BankAccountInformation
                {
                    AccountHolder = "Jane Smith",
                    FirstName = "Jane",
                    LastName = "Smith",
                    AccountType = BankAccountType.PersonalChecking,
                    RoutingNumber = "021000021",
                    AccountNumber = "123456789012"
                }
            };
            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(json);

            // Assert - MaskShowFirstAndLast4: 1234 (first 4) + XXXX (4 middle digits) + 9012 (last 4)
            Assert.Contains("XXXXXXXX9012", redacted);
            Assert.DoesNotContain("123456789012", redacted);
        }

        [Fact]
        public void RedactSensitiveData_WithBankAccountInformation_CompletelyRedactsRoutingNumber()
        {
            // Arrange
            var model = new CreateTokenRequest
            {
                Payer = "Jane Smith",
                EmailAddress = "jane@example.com",
                BankAccountInformation = new BankAccountInformation
                {
                    AccountHolder = "Jane Smith",
                    FirstName = "Jane",
                    LastName = "Smith",
                    AccountType = BankAccountType.PersonalChecking,
                    RoutingNumber = "021000021",
                    AccountNumber = "123456789012"
                }
            };
            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(json);

            // Assert
            Assert.Contains("\"XXXXXXXX9012\"", redacted);
            Assert.DoesNotContain("123456789012", redacted);
        }

        [Fact]
        public void RedactSensitiveData_PreservesNonSensitiveData()
        {
            // Arrange
            var model = new CreateTokenRequest
            {
                Payer = "Test User",
                EmailAddress = "test@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "Test User",
                    CardNumber = "4111111111111111",
                    Cvc = "123",
                    Month = "12",
                    Year = 2025,
                    PostalCode = "12345"
                }
            };
            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(json);

            // Assert - Non-sensitive data should remain
            Assert.Contains("Test User", redacted);
            Assert.Contains("test@example.com", redacted);
            Assert.Contains("12345", redacted);
        }

        [Fact]
        public void RedactSensitiveData_WithEmptyString_ReturnsEmptyString()
        {
            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData("");

            // Assert
            Assert.Equal("", redacted);
        }

        [Fact]
        public void RedactSensitiveData_WithNull_ReturnsNull()
        {
            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(null);

            // Assert
            Assert.Null(redacted);
        }

        [Fact]
        public void RedactSensitiveData_WithInvalidJson_ReturnsOriginal()
        {
            // Arrange
            var invalidJson = "not valid json {";

            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(invalidJson);

            // Assert
            Assert.Equal(invalidJson, redacted);
        }

        [Fact]
        public void RedactSensitiveData_WithNoSensitiveProperties_ReturnsUnchanged()
        {
            // Arrange
            var json = "{\"name\":\"Test\",\"amount\":100}";

            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(json);

            // Assert
            Assert.Equal(json, redacted);
        }

        [Fact]
        public void RedactSensitiveData_WithShortValue_MasksAppropriately()
        {
            // Arrange
            var model = new TestPaymentModel
            {
                Payer = "Test",
                CardNumber = "1234" // Only 4 digits
            };
            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(json, typeof(TestPaymentModel));

            // Assert - Should be fully masked since less than 8 digits
            Assert.Contains("\"XXXX\"", redacted);
        }

        [Fact]
        public void RedactSensitiveData_WithCreateTransactionRequest_RedactsProperly()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                Amount = 100.00m,
                Payer = "John Doe",
                EmailAddress = "john@example.com",
                CreditCardInformation = new CreditCardInformation
                {
                    AccountHolder = "John Doe",
                    CardNumber = "5454545454545454",
                    Cvc = "999",
                    Month = "12",
                    Year = 2034,
                    PostalCode = "12345"
                },
                Comments = "Test transaction"
            };
            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(json, typeof(CreateTransactionRequest));

            // Assert - MaskShowFirstAndLast4: 5454 (first 4) + XXXXXXXX (8 middle digits) + 5454 (last 4)
            Assert.Contains("XXXXXXXXXXXX5454", redacted);
            Assert.Contains("\"XXX\"", redacted);
            Assert.DoesNotContain("5454545454545454", redacted);
            Assert.DoesNotContain("\"999\"", redacted);
            // Non-sensitive fields preserved
            Assert.Contains("John Doe", redacted);
            Assert.Contains("john@example.com", redacted);
            Assert.Contains("100", redacted);
        }

        [Fact]
        public void RedactSensitiveData_HandlesNestedObjects()
        {
            // Arrange
            var request = new CreateTokenRequest
            {
                Payer = "Test User",
                EmailAddress = "test@example.com",
                BankAccountInformation = new BankAccountInformation
                {
                    AccountHolder = "Test User",
                    RoutingNumber = "021000021",
                    AccountNumber = "987654321098"
                }
            };
            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            // Act
            var redacted = SensitiveDataRedactor.RedactSensitiveData(json, typeof(CreateTokenRequest));

            // Assert - MaskShowFirstAndLast4: 9876 (first 4) + XXXX (4 middle digits) + 1098 (last 4)
            Assert.Contains("XXXXXXXX1098", redacted);
            Assert.DoesNotContain("987654321098", redacted);
        }
    }
}
