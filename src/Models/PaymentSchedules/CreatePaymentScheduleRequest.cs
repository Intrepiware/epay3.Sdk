using System;
using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Request model for creating a payment schedule for recurring payments.
    /// </summary>
    public class CreatePaymentScheduleRequest
    {
        /// <summary>
        /// Gets or sets the name of the payer that is shown on the receipt.
        /// Required.
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// Gets or sets the recipient of the emailed receipt.
        /// Required.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the token Id that represents the payment method to be used on the schedule.
        /// Required.
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// Gets or sets the amount of each recurring payment.
        /// Required. Maximum 10,000,000.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the interval by which the payments should be run.
        /// Required.
        /// </summary>
        public PaymentInterval Interval { get; set; }

        /// <summary>
        /// Gets or sets the number of days, weeks, etc to wait between payments.
        /// Required. Maximum 365, minimum 0.
        /// </summary>
        public int IntervalCount { get; set; }

        /// <summary>
        /// Gets or sets the date of the initial payment.
        /// If no date is set, the first payment will be run immediately.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the date of the last payment if the schedule is supposed to have an end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the number of payments to process on the schedule if the schedule
        /// is only supposed to have a pre-determined number of payments.
        /// Maximum 365, minimum 0.
        /// </summary>
        public int? NumberOfTotalPayments { get; set; }

        /// <summary>
        /// Gets or sets custom attribute values.
        /// The key in the dictionary is the identifier of the custom attribute.
        /// </summary>
        public Dictionary<string, string> AttributeValues { get; set; }

        /// <summary>
        /// Gets or sets comments that are shown on the receipt.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the fee being charged by the initiating party of this transaction.
        /// This does not include the standard transaction fees.
        /// Maximum 1,000,000, minimum 0.
        /// </summary>
        public double? InitiatingPartyFee { get; set; }

        /// <summary>
        /// Gets or sets the IP Address of the payer.
        /// </summary>
        public string IpAddress { get; set; }
    }
}
