namespace epay3.Sdk.Models
{
    /// <summary>
    /// Represents a division within a batch.
    /// </summary>
    public class Division
    {
        /// <summary>
        /// Gets or sets the division name.
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets or sets the amount for this division.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
