# epay3 SDK for .NET

Unofficial .NET SDK for the epay3 Payment API. This SDK was created by an ePay3 developer, but it is an experimental project meant to test building with [Claude Code](https://claude.ai). 

It is currently untested, and not recommended for use in Production environments.

## Features

- ✅ Clean, idiomatic C# API
- ✅ Full async/await support
- ✅ .NET Standard 2.0 for broad compatibility
- ✅ Strong typing with comprehensive models
- ✅ Resource-based organization
- ✅ Built-in authentication
- ✅ Comprehensive error handling

## Installation

```bash
dotnet add package epay3.Sdk
```

## Quick Start

```csharp
using epay3.Sdk;
using epay3.Sdk.Models;

// Initialize the client
var client = new epay3Client("your-api-key", "your-api-secret");

// Create a token
var tokenRequest = new CreateTokenRequest
{
    Payer = "John Doe",
    EmailAddress = "john@example.com",
    CreditCardInformation = new CreditCardInformation
    {
        AccountHolder = "John Doe",
        CardNumber = "4111111111111111",
        Cvc = "123",
        Month = "12",
        Year = 2025,
        PostalCode = "12345"
    }
};

var tokenId = await client.Tokens.CreateAsync(tokenRequest);

// Process a transaction
var transactionRequest = new CreateTransactionRequest
{
    Amount = 100.00,
    Payer = "John Doe",
    EmailAddress = "john@example.com",
    TokenId = tokenId,
    Comments = "Payment for services"
};

var response = await client.Transactions.CreateAsync(transactionRequest);
Console.WriteLine($"Transaction ID: {response.Id}");
```

## Supported Resources (Phase 1)

### Transactions
- **Get** - Retrieve transaction details
- **Create** - Process a sale transaction
- **Authorize** - Authorize a credit card
- **Refund** - Refund a transaction
- **Void** - Void a transaction

### Tokens
- **Get** - Retrieve token details
- **Create** - Store a payment method
- **Delete** - Remove a stored token

## Configuration

### Basic Configuration
```csharp
var client = new epay3Client("api-key", "api-secret");
```

### Advanced Configuration
```csharp
var options = new epay3ClientOptions
{
    ApiKey = "your-key",
    ApiSecret = "your-secret",
    BaseUrl = "https://api.epaypolicydemo.com",
    TimeoutSeconds = 30
};

var client = new epay3Client(options);
```

### Impersonation
```csharp
// Use impersonation header for processing on behalf of another account
await client.Transactions.CreateAsync(request, impersonationAccountKey: "account-key");
```

## Error Handling

```csharp
try
{
    var response = await client.Transactions.CreateAsync(request);
}
catch (AuthenticationException ex)
{
    // Invalid API credentials
    Console.WriteLine("Authentication failed");
}
catch (epay3ApiException ex)
{
    // API returned an error
    Console.WriteLine($"API Error: {ex.StatusCode} - {ex.ResponseBody}");
}
catch (epay3Exception ex)
{
    // Other SDK errors
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Development

### Building
```bash
dotnet build
```

### Running Tests
1. Update `tests/appsettings.json` with your UAT credentials
2. Run tests:
```bash
dotnet test
```

## Project Structure

```
src/
├── epay3Client.cs              # Main client
├── epay3ClientOptions.cs       # Configuration
├── Http/
│   └── HttpClientWrapper.cs    # HTTP communication
├── Resources/
│   ├── TransactionsResource.cs
│   └── TokensResource.cs
├── Models/
│   ├── Common/                 # Shared models
│   ├── Transactions/           # Transaction models
│   └── Tokens/                 # Token models
└── Exceptions/                 # Custom exceptions

tests/
├── TransactionsResourceTests.cs
├── TokensResourceTests.cs
└── appsettings.json           # UAT credentials
```

## Roadmap

### Phase 1 (Completed) ✅
- Core infrastructure
- Transactions resource
- Tokens resource
- Integration tests

### Phase 2 (Planned)
- AutoPay resource
- Invoices resource
- ManagedInvoices resource
- PaymentSchedules resource
- Batches resource
- TransactionFees resource
- TokenPageSessions resource
- IvrSessions resource

### Phase 3 (Planned)
- Comprehensive documentation
- NuGet package
- Additional error handling
- Retry policies

## License

MIT

## Support

For issues and questions:
- GitHub Issues: [Repository URL]
- Email: support@epay3.com
