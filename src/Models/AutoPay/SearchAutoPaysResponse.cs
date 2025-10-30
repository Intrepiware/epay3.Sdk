using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response model for AutoPay search results.
    /// </summary>
    public class SearchAutoPaysResponse
    {
        /// <summary>
        /// Gets or sets the list of AutoPays.
        /// </summary>
        public List<AutoPayListItem> AutoPays { get; set; }

        /// <summary>
        /// Gets or sets the total number of records matching the search criteria.
        /// </summary>
        public int TotalRecords { get; set; }
    }
}
