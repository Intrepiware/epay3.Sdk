namespace epay3.Sdk.Models
{
    /// <summary>
    /// Request model for refunding a transaction.
    /// </summary>
    public class RefundTransactionRequest
    {
        /// <summary>
        /// Gets or sets the amount to refund.
        /// If null, a full refund will be processed.
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// Gets or sets whether to send an e-receipt to all parties upon successful refund.
        /// </summary>
        public bool? SendReceipt { get; set; }
    }
}
