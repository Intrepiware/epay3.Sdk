using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using epay3.Sdk.Http;
using epay3.Sdk.Models;

namespace epay3.Sdk.Resources
{
    /// <summary>
    /// Resource for managing batches.
    /// </summary>
    public class BatchesResource
    {
        private readonly HttpClientWrapper _http;

        internal BatchesResource(HttpClientWrapper http)
        {
            _http = http;
        }

        /// <summary>
        /// Gets a collection of batches.
        /// </summary>
        /// <param name="page">The page of results to return. Default is 1.</param>
        /// <param name="impersonationAccountKey">Optional account key for impersonation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response containing the collection of batches.</returns>
        public async Task<GetBatchesResponse> GetAsync(
            int? page = null,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            var queryParams = new System.Collections.Generic.Dictionary<string, string>();

            if (page.HasValue)
            {
                queryParams["page"] = page.Value.ToString();
            }

            var queryString = queryParams.Count > 0
                ? "?" + string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"))
                : string.Empty;

            return await _http.GetAsync<GetBatchesResponse>(
                $"/api/v1/batches{queryString}",
                impersonationAccountKey,
                cancellationToken);
        }
    }
}
