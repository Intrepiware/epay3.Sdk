using System;
using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response model providing details of an AutoPay.
    /// </summary>
    public class AutoPayResponse
    {
        /// <summary>
        /// Gets or sets the Id of the AutoPay.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the token Id that represents the payment method to be used on the schedule.
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// Gets or sets the attributes associated with the AutoPay.
        /// </summary>
        public Dictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the AutoPay.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the create date of the auto pay subscription.
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the cancel date of the auto pay subscription.
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// Gets or sets the finance quote number associated with the AutoPay, if applicable.
        /// </summary>
        public string FinanceQuoteNumber { get; set; }

        /// <summary>
        /// Gets or sets the Id of the transaction that represents the down payment for the AutoPay, if applicable.
        /// </summary>
        public long? FinanceDownPaymentTransactionId { get; set; }
    }
}
