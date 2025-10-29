using System;

namespace epay3.Sdk
{
    /// <summary>
    /// Configuration options for the epay3 API client.
    /// </summary>
    public class epay3ClientOptions
    {
        /// <summary>
        /// Gets or sets the API key for authentication.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the API secret for authentication.
        /// </summary>
        public string ApiSecret { get; set; }

        /// <summary>
        /// Gets or sets the base URL for the API.
        /// Default is https://api.epaypolicydemo.com for the demo environment.
        /// </summary>
        public string BaseUrl { get; set; } = "https://api.epaypolicydemo.com";

        /// <summary>
        /// Gets or sets the timeout for HTTP requests in seconds.
        /// Default is 30 seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// Validates the configuration options.
        /// </summary>
        internal void Validate()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("ApiKey is required.", nameof(ApiKey));

            if (string.IsNullOrWhiteSpace(ApiSecret))
                throw new ArgumentException("ApiSecret is required.", nameof(ApiSecret));

            if (string.IsNullOrWhiteSpace(BaseUrl))
                throw new ArgumentException("BaseUrl is required.", nameof(BaseUrl));

            if (TimeoutSeconds <= 0)
                throw new ArgumentException("TimeoutSeconds must be greater than 0.", nameof(TimeoutSeconds));
        }
    }
}