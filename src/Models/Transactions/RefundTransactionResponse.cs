namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response model for transaction refund.
    /// </summary>
    public class RefundTransactionResponse
    {
        /// <summary>
        /// Gets or sets the ID of the newly created refund transaction.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the reversal response code.
        /// </summary>
        public ReversalResponseCode ReversalResponseCode { get; set; }
    }
}