namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response model for transaction creation.
    /// </summary>
    public class TransactionResponse
    {
        /// <summary>
        /// Gets or sets the ID of the transaction.
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
