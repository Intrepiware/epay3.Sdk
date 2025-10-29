using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace epay3.Sdk.Integration.Tests
{
    /// <summary>
    /// Configuration helper for integration tests.
    /// </summary>
    public static class TestConfiguration
    {
        private static readonly IConfiguration Configuration;

        static TestConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        /// <summary>
        /// Creates a configured epay3Client for testing.
        /// </summary>
        /// <returns>A configured client instance.</returns>
        public static epay3Client CreateClient()
        {
            var apiKey = Configuration["epay3:ApiKey"];
            var apiSecret = Configuration["epay3:ApiSecret"];
            var baseUrl = Configuration["epay3:BaseUrl"];

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiSecret))
            {
                throw new InvalidOperationException(
                    "API credentials are not configured. Please update appsettings.json with your UAT credentials.");
            }

            return new epay3Client(apiKey, apiSecret, baseUrl);
        }

        /// <summary>
        /// Gets a configuration value.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <returns>The configuration value.</returns>
        public static string GetValue(string key)
        {
            return Configuration[key];
        }
    }
}
