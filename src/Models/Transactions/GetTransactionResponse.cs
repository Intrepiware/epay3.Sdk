using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response model containing transaction details.
    /// </summary>
    public class GetTransactionResponse
    {
        /// <summary>
        /// Gets or sets the ID of the transaction.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the public ID of the transaction.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the name of the payer.
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// Gets or sets the email address of the payer.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the type of transaction.
        /// </summary>
        public TransactionType TransactionType { get; set; }

     /// <summary>
        /// Gets or sets the total amount charged to the payer including all fees.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
  /// Gets or sets the transaction fee charged.
  /// </summary>
        public decimal Fee { get; set; }

      /// <summary>
  /// Gets or sets the fee charged to the payer.
        /// </summary>
        public decimal PayerFee { get; set; }

        /// <summary>
        /// Gets or sets the masked account number.
        /// </summary>
        public string MaskedAccountNumber { get; set; }

        /// <summary>
        /// Gets or sets comments from the payer.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the ID of the original transaction for refunds/returns.
        /// </summary>
        public long? OriginalTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the financing account.
        /// </summary>
        public string FinancingAccount { get; set; }

        /// <summary>
        /// Gets or sets custom attribute values.
        /// </summary>
        public List<AttributeValue> AttributeValues { get; set; }
    }
}
