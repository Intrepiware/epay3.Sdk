using System;
using epay3.Sdk.Http;
using epay3.Sdk.Resources;

namespace epay3.Sdk
{
    /// <summary>
    /// Main client for interacting with the epay3 API.
    /// </summary>
    public class epay3Client : IDisposable
    {
        private readonly HttpClientWrapper _httpClient;
        private bool _disposed;

        /// <summary>
        /// Gets the Transactions resource for transaction-related operations.
        /// </summary>
        public TransactionsResource Transactions { get; }

        /// <summary>
        /// Gets the Tokens resource for token-related operations.
        /// </summary>
        public TokensResource Tokens { get; }

        /// <summary>
        /// Gets the AutoPay resource for AutoPay-related operations.
        /// </summary>
        public AutoPayResource AutoPay { get; }

        /// <summary>
        /// Gets the PaymentSchedules resource for payment schedule-related operations.
        /// </summary>
        public PaymentSchedulesResource PaymentSchedules { get; }

        /// <summary>
        /// Gets the TransactionFees resource for transaction fee calculation operations.
        /// </summary>
        public TransactionFeesResource TransactionFees { get; }

        /// <summary>
        /// Gets the Batches resource for batch-related operations.
        /// </summary>
        public BatchesResource Batches { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="epay3Client"/> class.
        /// </summary>
        /// <param name="apiKey">The API key for authentication.</param>
        /// <param name="apiSecret">The API secret for authentication.</param>
        /// <param name="baseUrl">The base URL for the API. Defaults to the demo environment.</param>
        public epay3Client(string apiKey, string apiSecret, string baseUrl = "https://api.epaypolicydemo.com")
            : this(new epay3ClientOptions
            {
                ApiKey = apiKey,
                ApiSecret = apiSecret,
                BaseUrl = baseUrl
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="epay3Client"/> class.
        /// </summary>
        /// <param name="options">The client configuration options.</param>
        public epay3Client(epay3ClientOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _httpClient = new HttpClientWrapper(options);

            // Initialize resources
            Transactions = new TransactionsResource(_httpClient);
            Tokens = new TokensResource(_httpClient);
            AutoPay = new AutoPayResource(_httpClient);
            PaymentSchedules = new PaymentSchedulesResource(_httpClient);
            TransactionFees = new TransactionFeesResource(_httpClient);
            Batches = new BatchesResource(_httpClient);
        }

        /// <summary>
        /// Disposes the client and releases resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}
