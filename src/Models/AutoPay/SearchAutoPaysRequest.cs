using System;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// Request model for searching AutoPays.
    /// </summary>
    public class SearchAutoPaysRequest
    {
        /// <summary>
        /// Gets or sets when filtering by create date, the earliest permitted date.
        /// Default is 30 days ago.
        /// </summary>
        public DateTime? CreateDateStart { get; set; }

        /// <summary>
        /// Gets or sets when filtering by create date, the latest permitted date.
        /// Default is now.
        /// </summary>
        public DateTime? CreateDateEnd { get; set; }

        /// <summary>
        /// Gets or sets when filtering by cancel date, the earliest permitted date.
        /// Default is 30 days ago.
        /// </summary>
        public DateTime? CancelDateStart { get; set; }

        /// <summary>
        /// Gets or sets when filtering by cancel date, the latest permitted date.
        /// Default is now.
        /// </summary>
        public DateTime? CancelDateEnd { get; set; }

        /// <summary>
        /// Gets or sets the page number for pagination.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Gets or sets the number of results per page.
        /// </summary>
        public int? ItemsPerPage { get; set; }
    }
}
