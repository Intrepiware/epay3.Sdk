namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response model for transaction authorization.
    /// </summary>
    public class AuthorizeTransactionResponse
    {
        /// <summary>
        /// Gets or sets the ID of the authorization.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the message from the payment processor.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the payment response code.
        /// </summary>
        public PaymentResponseCode PaymentResponseCode { get; set; }
    }
}