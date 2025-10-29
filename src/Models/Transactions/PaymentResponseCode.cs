using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response codes for payment processing.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentResponseCode
    {
        /// <summary>Generic decline</summary>
        GenericDecline,

        /// <summary>Payment succeeded</summary>
        Success,

        /// <summary>Do not honor</summary>
        DoNotHonor,

        /// <summary>Invalid account number</summary>
        InvalidAccountNumber,

        /// <summary>Insufficient funds</summary>
        InsufficientFunds,

        /// <summary>CVV validation failed</summary>
        DeclineCvvFail,

        /// <summary>Exceeds approval amount limit</summary>
        ExceedsApprovalAmountLimit,

        /// <summary>No such issuer</summary>
        NoSuchIssuer,

        /// <summary>Invalid payment type</summary>
        InvalidPaymentType,

        /// <summary>Invalid expiration date</summary>
        InvalidExpirationDate,

        /// <summary>Lost or stolen card</summary>
        LostOrStolenCard,

        /// <summary>Expired card</summary>
        ExpiredCard,

        /// <summary>Hard duplicate transaction</summary>
        HardDuplicateTransaction,

        /// <summary>Invalid token</summary>
        InvalidToken,

        /// <summary>Invalid authorization</summary>
        InvalidAuthorization,

        /// <summary>Invalid routing number</summary>
        InvalidRoutingNumber,

        /// <summary>Soft duplicate transaction</summary>
        SoftDuplicateTransaction,

        /// <summary>Cached payment data expired</summary>
        CachedPaymentDataExpired,

        /// <summary>Payment method is blacklisted</summary>
        PaymentMethodBlackListed
    }
}
