using System.Threading;
using System.Threading.Tasks;
using epay3.Sdk.Http;
using epay3.Sdk.Models;

namespace epay3.Sdk.Resources
{
    /// <summary>
    /// Resource for calculating transaction fees.
    /// </summary>
    public class TransactionFeesResource
    {
        private readonly HttpClientWrapper _http;

        internal TransactionFeesResource(HttpClientWrapper http)
        {
            _http = http;
        }

     /// <summary>
        /// Calculates and returns transaction fees for a given net amount due based on the account.
   /// </summary>
        /// <param name="amount">Net amount due from which to calculate the transaction fees.</param>
      /// <param name="impersonationAccountKey">Optional account key for impersonation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The calculated transaction fees for ACH and credit card payments.</returns>
        public async Task<GetTransactionFeesResponse> GetAsync(
  decimal amount,
      string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.GetAsync<GetTransactionFeesResponse>(
                "/api/v1/transactionFees",
                new { amount },
                impersonationAccountKey,
                cancellationToken);
        }
    }
}