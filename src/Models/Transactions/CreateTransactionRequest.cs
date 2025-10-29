using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Request model for creating a transaction (sale).
    /// </summary>
    public class CreateTransactionRequest
    {
        /// <summary>
        /// Gets or sets the total amount to charge not including any payer fees.
        /// Required.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the name of the payer shown on the receipt.
        /// Required.
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// Gets or sets the email address for the receipt.
        /// Required.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the pre-calculated payer fee.
        /// If set, the fee will not be re-calculated.
        /// </summary>
        public double? PayerFee { get; set; }

        /// <summary>
        /// Gets or sets comments shown on the receipt.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the token ID to reference a previously stored payment token.
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// Gets or sets credit card information for credit card transactions.
        /// </summary>
        public CreditCardInformation CreditCardInformation { get; set; }

        /// <summary>
        /// Gets or sets bank account information for eCheck/ACH transactions.
        /// </summary>
        public BankAccountInformation BankAccountInformation { get; set; }

        /// <summary>
        /// Gets or sets the authorization ID when capturing a previously authorized transaction.
        /// </summary>
        public string AuthorizationId { get; set; }

        /// <summary>
        /// Gets or sets whether to send an e-receipt to the payer and account holders.
        /// </summary>
        public bool? SendReceipt { get; set; }

        /// <summary>
        /// Gets or sets the fee charged by the initiating party.
        /// This does not include standard transaction fees.
        /// </summary>
        public double? InitiatingPartyFee { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the payer.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the currency for the transaction.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets custom attribute values.
        /// Dictionary key is the identifier of the custom attribute.
        /// </summary>
        public Dictionary<string, string> AttributeValues { get; set; }
    }
}
