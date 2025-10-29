namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response model for transaction void.
    /// </summary>
    public class VoidTransactionResponse
    {
        /// <summary>
        /// Gets or sets the reversal response code.
        /// </summary>
        public ReversalResponseCode ReversalResponseCode { get; set; }
    }
}
