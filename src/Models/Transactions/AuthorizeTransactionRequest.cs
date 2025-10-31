namespace epay3.Sdk.Models
{
    /// <summary>
    /// Request model for authorizing a credit card transaction.
    /// </summary>
    public class AuthorizeTransactionRequest
    {
        /// <summary>
        /// Gets or sets the token ID to reference a previously stored payment token.
        /// Required.
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
   /// Gets or sets the total amount to authorize.
        /// Required.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
