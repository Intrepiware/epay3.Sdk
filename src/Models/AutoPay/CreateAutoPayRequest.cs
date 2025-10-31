using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Request model for creating an AutoPay.
    /// </summary>
    public class CreateAutoPayRequest
    {
        /// <summary>
        /// Gets or sets the public token Id used to reference a previously stored payment token.
        /// </summary>
        public string PublicTokenId { get; set; }

        /// <summary>
        /// Gets or sets custom attribute values.
        /// The key in the dictionary is the identifier of the custom attribute.
        /// </summary>
        public Dictionary<string, string> AttributeValues { get; set; }

        /// <summary>
        /// Gets or sets the recipient of the emailed receipt.
        /// Required.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the name of the payer.
        /// </summary>
        public string Payer { get; set; }
    }
}