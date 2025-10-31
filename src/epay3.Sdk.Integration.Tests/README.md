# epay3 SDK Integration Tests

This project contains integration tests for the epay3 SDK that run against the UAT environment.

## Setup

1. **Update credentials in appsettings.json:**
   Edit `appsettings.json` and add your UAT API credentials:
   ```json
   {
     "epay3": {
       "ApiKey": "your-actual-uat-api-key",
       "ApiSecret": "your-actual-uat-api-secret",
       "BaseUrl": "https://api.epaypolicydemo.com"
     }
   }
   ```

2. **Run the tests:**
   ```bash
   dotnet test
   ```

## Test Coverage

### TokensResource Tests
- `CreateToken_WithCreditCard_ReturnsTokenId` - Tests token creation with credit card
- `CreateToken_WithBankAccount_ReturnsTokenId` - Tests token creation with bank account
- `GetToken_WithValidId_ReturnsToken` - Tests retrieving token details
- `DeleteToken_WithValidId_Succeeds` - Tests token deletion

### TransactionsResource Tests
- `CreateTransaction_WithCreditCard_ReturnsSuccess` - Tests transaction with credit card
- `CreateTransaction_WithToken_ReturnsSuccess` - Tests transaction using a token
- `GetTransaction_WithValidId_ReturnsTransaction` - Tests retrieving transaction details
- `AuthorizeTransaction_WithToken_ReturnsSuccess` - Tests credit card authorization
- `VoidTransaction_WithValidId_ReturnsSuccess` - Tests voiding a transaction

## Notes

- Tests use standard test credit card numbers (e.g., 4111111111111111)
- Tests create real data in the UAT environment
- Some tests depend on transaction state timing and may occasionally fail if the API hasn't fully processed the transaction
