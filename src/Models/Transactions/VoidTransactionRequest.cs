namespace epay3.Sdk.Models
{
    /// <summary>
    /// Request model for voiding a transaction.
    /// </summary>
    public class VoidTransactionRequest
    {
        /// <summary>
        /// Gets or sets whether to send an e-receipt to all parties upon successful void.
        /// </summary>
        public bool? SendReceipt { get; set; }
    }
}
