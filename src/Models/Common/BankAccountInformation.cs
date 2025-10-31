using epay3.Sdk.Attributes;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Bank account information for processing ACH/eCheck payments or storing tokens.
    /// </summary>
    public class BankAccountInformation
    {
        /// <summary>
        /// Gets or sets the name that is on the bank account.
        /// </summary>
        public string AccountHolder { get; set; }

        /// <summary>
        /// Gets or sets the first name of the person authorizing the transaction.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the person authorizing the transaction.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the type of bank account.
        /// </summary>
        public BankAccountType AccountType { get; set; }

        /// <summary>
        /// Gets or sets the 9-digit routing number.
        /// </summary>
        public string RoutingNumber { get; set; }

        /// <summary>
        /// Gets or sets the bank account number.
        /// </summary>
        [SensitiveData(RedactionMode.MaskShowLast4)]
        public string AccountNumber { get; set; }
    }
}
