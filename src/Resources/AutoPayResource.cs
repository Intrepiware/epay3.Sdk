using epay3.Sdk.Http;
using epay3.Sdk.Models;
using System.Threading;
using System.Threading.Tasks;

namespace epay3.Sdk.Resources
{
    /// <summary>
    /// Provides methods for interacting with AutoPay endpoints.
    /// </summary>
    public class AutoPayResource
    {
        private readonly HttpClientWrapper _http;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPayResource"/> class.
        /// </summary>
        /// <param name="http">The HTTP client wrapper for API communication.</param>
        internal AutoPayResource(HttpClientWrapper http)
        {
            _http = http;
        }

        /// <summary>
        /// Retrieves the details of an AutoPay.
        /// </summary>
        /// <param name="id">The Id of the AutoPay.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The AutoPay details.</returns>
        public async Task<AutoPayResponse> GetAsync(
            long id,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.GetAsync<AutoPayResponse>(
                $"/api/v1/autoPay/{id}",
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves a list of AutoPays based on search parameters.
        /// </summary>
        /// <param name="request">The search parameters.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The search results.</returns>
        public async Task<SearchAutoPaysResponse> SearchAsync(
            SearchAutoPaysRequest request = null,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.GetAsync<SearchAutoPaysResponse>(
                "/api/v1/autoPays",
                request,
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Creates an AutoPay.
        /// </summary>
        /// <param name="request">The AutoPay creation request.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The Id of the created AutoPay.</returns>
        public async Task<long> CreateAsync(
            CreateAutoPayRequest request,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.PostAsync(
                "/api/v1/autoPay",
                request,
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Cancels an active AutoPay.
        /// </summary>
        /// <param name="id">The Id of the AutoPay.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task CancelAsync(
            long id,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            await _http.PostAsync<object>(
                $"/api/v1/autoPay/{id}/cancel",
                null,
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Reactivates a cancelled AutoPay.
        /// </summary>
        /// <param name="id">The Id of the AutoPay.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task RestartAsync(
            long id,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            await _http.PostAsync<object>(
                $"/api/v1/autoPay/{id}/restart",
                null,
                impersonationAccountKey,
                cancellationToken);
        }
    }
}