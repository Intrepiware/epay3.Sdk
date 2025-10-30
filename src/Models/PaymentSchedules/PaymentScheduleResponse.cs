using System;
using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response model providing details of a payment schedule.
    /// </summary>
    public class PaymentScheduleResponse
    {
        /// <summary>
        /// Gets or sets the public Id of the payment schedule.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the payer that is shown on the receipt.
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// Gets or sets the recipient of the emailed receipt.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the token Id that represents the payment method to be used on the schedule.
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// Gets or sets the number of payments to process on the schedule
        /// if the payment schedule has a pre-determined list of payments.
        /// </summary>
        public int? NumberOfTotalPayments { get; set; }

        /// <summary>
        /// Gets or sets the number of executed payments.
        /// </summary>
        public int? NumberOfExecutedPayments { get; set; }

        /// <summary>
        /// Gets or sets the amount of each recurring payment.
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// Gets or sets the payer fee.
        /// Used if the calling application has pre-calculated a payer fee.
        /// In that case, the fee will not be re-calculated.
        /// This amount, if set, will not be added to the amount field prior to processing.
        /// </summary>
        public double? PayerFee { get; set; }

        /// <summary>
        /// Gets or sets the date of the initial payment.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the schedule if the schedule was created with a pre-determined end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the date of the next payment.
        /// </summary>
        public DateTime? NextPaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the interval by which the payments should be run.
        /// </summary>
        public PaymentInterval? Interval { get; set; }

        /// <summary>
        /// Gets or sets the number of days, weeks, etc to wait between payments.
        /// </summary>
        public int? IntervalCount { get; set; }

        /// <summary>
        /// Gets or sets custom attribute values.
        /// The key in the dictionary is the identifier of the custom attribute.
        /// </summary>
        public Dictionary<string, string> AttributeValues { get; set; }

        /// <summary>
        /// Gets or sets comments that are shown on the receipt.
        /// </summary>
        public string Comments { get; set; }
    }
}
