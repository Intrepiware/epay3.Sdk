using epay3.Sdk.Attributes;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Credit card information for processing payments or storing tokens.
    /// </summary>
    public class CreditCardInformation
    {
        /// <summary>
        /// Gets or sets the name that is on the credit card account.
        /// </summary>
        public string AccountHolder { get; set; }

        /// <summary>
        /// Gets or sets the credit card number.
        /// </summary>
        [SensitiveData(RedactionMode.MaskShowLast4)]
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the security code for the credit card (CVV/CVC).
        /// </summary>
        [SensitiveData(RedactionMode.Complete, RedactedValue = "XXX")]
        public string Cvc { get; set; }

        /// <summary>
        /// Gets or sets the expiration month (1-12).
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets the expiration year (YYYY).
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the postal code for the credit card.
        /// </summary>
        public string PostalCode { get; set; }
    }
}