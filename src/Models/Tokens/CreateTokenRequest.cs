using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Request model for creating a payment token.
    /// </summary>
    public class CreateTokenRequest
    {
        /// <summary>
        /// Gets or sets the name of the payer that is storing the token.
        /// Required.
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// Gets or sets the email address of the payer.
        /// Required.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the credit card information for credit card tokens.
        /// Either this or BankAccountInformation must be provided.
        /// </summary>
        public CreditCardInformation CreditCardInformation { get; set; }

        /// <summary>
        /// Gets or sets the bank account information for eCheck/ACH tokens.
        /// Either this or CreditCardInformation must be provided.
        /// </summary>
        public BankAccountInformation BankAccountInformation { get; set; }

        /// <summary>
        /// Gets or sets custom attribute values.
        /// Dictionary key is the identifier of the custom attribute.
        /// </summary>
        public Dictionary<string, string> AttributeValues { get; set; }
    }
}