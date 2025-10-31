# Sensitive Data Redaction

The epay3 SDK includes an attribute-based system for redacting sensitive payment information from logs to prevent accidental exposure of credit card numbers, CVCs, and bank account details in production environments.

## How It Works

Sensitive properties are marked with the `[SensitiveData]` attribute, which tells the logging system how to redact the data when logging HTTP requests and responses.

## Redaction Modes

The SDK supports three redaction modes:

### 1. Complete Redaction
Completely replaces the value with a placeholder (e.g., "XXX" for CVCs).

```csharp
[SensitiveData(RedactionMode.Complete, RedactedValue = "XXX")]
public string Cvc { get; set; }
```

**Example:**
- Original: `"cvc": "123"`
- Redacted: `"cvc": "XXX"`

### 2. Mask Show Last 4
Masks all but the last 4 characters (useful for partial identification).

```csharp
[SensitiveData(RedactionMode.MaskShowLast4)]
public string CardNumber { get; set; }
```

**Example:**
- Original: `"cardNumber": "4111111111111111"`
- Redacted: `"cardNumber": "XXXXXXXXXXXX1111"`

### 3. Mask Show First and Last 4 (Default for Payment Numbers)
Shows the first 4 and last 4 characters, masking the middle (best balance between security and identification).

```csharp
[SensitiveData(RedactionMode.MaskShowFirstAndLast4)]
public string CardNumber { get; set; }
```

**Example:**
- Original: `"cardNumber": "4111111111111111"`
- Redacted: `"cardNumber": "4111XXXXXXXX1111"`

## Built-in Redaction

The following properties are already configured with redaction:

### Credit Card Information
- **CardNumber**: `MaskShowFirstAndLast4` - Shows first 4 and last 4 digits (e.g., `4111XXXXXXXX1111`)
- **Cvc**: `Complete` - Completely redacted to `XXX`

### Bank Account Information
- **AccountNumber**: `MaskShowFirstAndLast4` - Shows first 4 and last 4 digits (e.g., `1234XXXX9012`)
- **RoutingNumber**: `Complete` - Completely redacted to `XXXXXXXXX`

## Enabling Logging

Verbose logging (including redacted sensitive data) can be enabled via the client options:

```csharp
var options = new epay3ClientOptions
{
    ApiKey = "your-api-key",
    ApiSecret = "your-api-secret",
    VerboseLogging = true // Enable logging with automatic redaction
};

var client = new epay3Client(options);
```

## Example Log Output

### Request (with sensitive data redacted):
```
[a1b2c3d4] HTTP REQUEST
================================================================================
POST https://api.epaypolicydemo.com/api/v1/transactions

Headers:
  Authorization: [REDACTED]
  Accept: application/json

Body:
{
  "amount": 100.50,
  "payer": "John Doe",
  "emailAddress": "john@example.com",
"creditCardInformation": {
    "accountHolder": "John Doe",
    "cardNumber": "4111XXXXXXXX1111",
    "cvc": "XXX",
    "month": "12",
    "year": 2025,
    "postalCode": "12345"
  }
}
```

## Adding Redaction to Custom Properties

If you extend the SDK models or create custom models, you can add redaction to your properties:

```csharp
public class CustomPaymentModel
{
    public string Payer { get; set; }
  
    // Completely redact SSN
    [SensitiveData(RedactionMode.Complete, RedactedValue = "***-**-****")]
    public string SocialSecurityNumber { get; set; }
    
    // Show last 4 digits of account number
    [SensitiveData(RedactionMode.MaskShowLast4)]
    public string AccountNumber { get; set; }
    
    // Custom mask character
    [SensitiveData(RedactionMode.MaskShowFirstAndLast4, MaskCharacter = '*')]
    public string CustomField { get; set; }
}
```

## Security Best Practices

1. **Always enable logging only in non-production environments** or ensure logs are stored securely
2. **Review log output** before enabling in production to ensure all sensitive data is properly redacted
3. **Use Complete redaction** for highly sensitive data like CVCs, SSNs, and passwords
4. **Use Mask modes** for data that needs partial identification (like card numbers for support)
5. **Regularly audit** your models to ensure new sensitive properties have appropriate attributes

## Customization

### Custom Redaction Values
```csharp
[SensitiveData(RedactionMode.Complete, RedactedValue = "[HIDDEN]")]
public string SecretValue { get; set; }
```

### Custom Mask Character
```csharp
[SensitiveData(RedactionMode.MaskShowLast4, MaskCharacter = '*')]
public string MaskedValue { get; set; }
```

## Implementation Details

- Redaction happens **before** logging, so sensitive data never reaches the log output
- Redaction uses **reflection and caching** for performance
- Redaction works with **nested objects** automatically
- Invalid JSON is returned unchanged (failsafe behavior)
- Redaction is **convention-based** - properties with the same name across types share redaction rules

## Testing

The SDK includes comprehensive tests for redaction behavior. See `SensitiveDataRedactorTests.cs` for examples of:
- Credit card redaction
- Bank account redaction
- Nested object handling
- Custom redaction modes
- Edge cases (empty strings, null values, invalid JSON)
