using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response model containing token details.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Gets or sets the public ID of the token.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the payer.
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// Gets or sets the email address of the payer.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the type of transaction/payment method.
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the masked account number for display.
        /// </summary>
        public string MaskedAccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the country of the token.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets custom attribute values.
        /// </summary>
        public List<AttributeValue> AttributeValues { get; set; }
    }
}