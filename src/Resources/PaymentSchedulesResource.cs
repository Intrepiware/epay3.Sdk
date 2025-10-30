using System.Threading;
using System.Threading.Tasks;
using epay3.Sdk.Http;
using epay3.Sdk.Models;

namespace epay3.Sdk.Resources
{
    /// <summary>
    /// Provides methods for interacting with payment schedule endpoints.
    /// </summary>
    public class PaymentSchedulesResource
    {
        private readonly HttpClientWrapper _http;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentSchedulesResource"/> class.
        /// </summary>
        /// <param name="http">The HTTP client wrapper for API communication.</param>
        internal PaymentSchedulesResource(HttpClientWrapper http)
        {
            _http = http;
        }

        /// <summary>
        /// Retrieves the details of a payment schedule.
        /// </summary>
        /// <param name="id">The public Id of the payment schedule.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The payment schedule details.</returns>
        public async Task<PaymentScheduleResponse> GetAsync(
            string id,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.GetAsync<PaymentScheduleResponse>(
                $"/api/v1/paymentSchedules/{id}",
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Creates a payment schedule for delayed payment or recurring payments.
        /// </summary>
        /// <param name="request">The payment schedule creation request.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The Id of the created payment schedule.</returns>
        public async Task<string> CreateAsync(
            CreatePaymentScheduleRequest request,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            return await _http.PostAsyncString(
                "/api/v1/paymentSchedules",
                request,
                impersonationAccountKey,
                cancellationToken);
        }

        /// <summary>
        /// Cancels an active payment schedule.
        /// </summary>
        /// <param name="id">The public Id of the payment schedule.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task CancelAsync(
            string id,
            string impersonationAccountKey = null,
            CancellationToken cancellationToken = default)
        {
            await _http.PostAsync<object>(
                $"/api/v1/paymentSchedules/{id}/cancel",
                null,
                impersonationAccountKey,
                cancellationToken);
        }
    }
}
