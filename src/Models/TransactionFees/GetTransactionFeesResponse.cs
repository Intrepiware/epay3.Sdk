namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response containing calculated transaction fees for ACH and credit card payments.
    /// </summary>
    public class GetTransactionFeesResponse
    {
        /// <summary>
        /// Gets or sets the ACH payer fee for the specified transaction amount.
        /// </summary>
        public decimal AchPayerFee { get; set; }

        /// <summary>
        /// Gets or sets the credit card payer fee for the specified transaction amount.
        /// </summary>
        public decimal CreditCardPayerFee { get; set; }
    }
}