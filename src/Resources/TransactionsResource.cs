using System.Threading;
using System.Threading.Tasks;
using epay3.Sdk.Http;
using epay3.Sdk.Models;

namespace epay3.Sdk.Resources
{
    /// <summary>
    /// Resource for managing transactions.
    /// </summary>
    public class TransactionsResource
    {
        private readonly HttpClientWrapper _http;

        internal TransactionsResource(HttpClientWrapper http)
        {
            _http = http;
        }

        /// <summary>
        /// Retrieves the details of a transaction.
        /// </summary>
        /// <param name="id">The ID of the transaction.</param>
        /// <param name="impersonationAccountKey">Optional account key for impersonation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The transaction details.</returns>
        public async Task<GetTransactionResponse> GetAsync(
            long id,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.GetAsync<GetTransactionResponse>(
                $"/api/v1/transactions/{id}",
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Processes a sale transaction for either ACH or credit card.
        /// </summary>
        /// <param name="request">The transaction request.</param>
        /// <param name="impersonationAccountKey">Optional account key for impersonation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The transaction response containing the ID and status.</returns>
        public async Task<TransactionResponse> CreateAsync(
            CreateTransactionRequest request,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.PostAsync<TransactionResponse>(
                "/api/v1/transactions",
                request,
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Creates an authorization on a credit card.
        /// </summary>
        /// <param name="request">The authorization request.</param>
        /// <param name="impersonationAccountKey">Optional account key for impersonation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The authorization response containing the ID and status.</returns>
        public async Task<AuthorizeTransactionResponse> AuthorizeAsync(
            AuthorizeTransactionRequest request,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.PostAsync<AuthorizeTransactionResponse>(
                "/api/v1/transactions/authorize",
                request,
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Processes a refund of a transaction.
        /// </summary>
        /// <param name="id">The ID of the transaction to refund.</param>
        /// <param name="request">The refund request.</param>
        /// <param name="impersonationAccountKey">Optional account key for impersonation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The refund response containing the new refund transaction ID.</returns>
        public async Task<RefundTransactionResponse> RefundAsync(
            long id,
            RefundTransactionRequest request,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.PostAsync<RefundTransactionResponse>(
                $"/api/v1/transactions/{id}/refund",
                request,
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Processes a void of a transaction.
        /// </summary>
        /// <param name="id">The ID of the transaction to void.</param>
        /// <param name="request">The void request.</param>
        /// <param name="impersonationAccountKey">Optional account key for impersonation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The void response.</returns>
        public async Task<VoidTransactionResponse> VoidAsync(
            long id,
            VoidTransactionRequest request,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.PostAsync<VoidTransactionResponse>(
                $"/api/v1/transactions/{id}/void",
                request,
                impersonationAccountKey,
                cancellationToken);
        }
    }
}
