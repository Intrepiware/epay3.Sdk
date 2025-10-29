using System.Threading;
using System.Threading.Tasks;
using epay3.Sdk.Http;
using epay3.Sdk.Models;

namespace epay3.Sdk.Resources
{
    /// <summary>
    /// Resource for managing payment tokens.
    /// </summary>
    public class TokensResource
    {
        private readonly HttpClientWrapper _http;

        internal TokensResource(HttpClientWrapper http)
        {
            _http = http;
        }

        /// <summary>
        /// Retrieves the details of a token.
        /// </summary>
        /// <param name="id">The public ID of the token.</param>
        /// <param name="impersonationAccountKey">Optional account key for impersonation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The token details.</returns>
        public async Task<TokenResponse> GetAsync(
            string id,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.GetAsync<TokenResponse>(
                $"/api/v1/tokens/{id}",
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Creates a payment token for either ACH or credit card payments.
        /// </summary>
        /// <param name="request">The token creation request.</param>
        /// <param name="impersonationAccountKey">Optional account key for impersonation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The ID of the created token.</returns>
        public async Task<string> CreateAsync(
            CreateTokenRequest request,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.PostAsyncString(
                "/api/v1/tokens",
                request,
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Deletes a stored token.
        /// </summary>
        /// <param name="id">The public ID of the token to delete.</param>
        /// <param name="impersonationAccountKey">Optional account key for impersonation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task DeleteAsync(
            string id,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            await _http.DeleteAsync(
                $"/api/v1/tokens/{id}",
                impersonationAccountKey,
                cancellationToken);
        }
    }
}
