using System;
using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Represents a batch list item in the batch collection.
    /// </summary>
    public class BatchListItem
    {
        /// <summary>
        /// Gets or sets the batch ID.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the batch was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the number of credit transactions in the batch.
        /// </summary>
        public int NumberOfCredits { get; set; }

        /// <summary>
        /// Gets or sets the total amount of credit transactions.
        /// </summary>
      public decimal TotalOfCredits { get; set; }

        /// <summary>
        /// Gets or sets the number of debit transactions in the batch.
        /// </summary>
        public int NumberOfDebits { get; set; }

        /// <summary>
        /// Gets or sets the total amount of debit transactions.
        /// </summary>
        public decimal TotalOfDebits { get; set; }

        /// <summary>
        /// Gets or sets the currency for the batch.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Gets or sets the divisions within the batch.
        /// </summary>
        public List<Division> Divisions { get; set; }

        /// <summary>
        /// Gets or sets the processor name.
        /// </summary>
        public string Processor { get; set; }

        /// <summary>
        /// Gets or sets the processor payment method.
        /// </summary>
        public string ProcessorPaymentMethod { get; set; }
    }
}
