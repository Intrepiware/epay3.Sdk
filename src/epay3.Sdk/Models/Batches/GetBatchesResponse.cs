using System.Collections.Generic;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response model containing a collection of batches.
    /// </summary>
    public class GetBatchesResponse
    {
        /// <summary>
        /// Gets or sets the list of batches.
        /// </summary>
        public List<BatchListItem> Batches { get; set; }

        /// <summary>
        /// Gets or sets the total number of batches available.
        /// </summary>
        public int TotalRecords { get; set; }
    }
}
