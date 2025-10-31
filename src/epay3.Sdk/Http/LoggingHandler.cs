using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace epay3.Sdk.Http
{
    /// <summary>
    /// HTTP message handler that logs requests and responses to the console for debugging.
    /// Sensitive payment information (credit card numbers, CVCs, account numbers) is redacted.
    /// </summary>
    internal class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid().ToString("N").Substring(0, 8);

            // Log request
            await LogRequest(request, requestId);

            // Call the inner handler
            var startTime = DateTime.UtcNow;
            HttpResponseMessage response = null;
            Exception exception = null;

            try
            {
                response = await base.SendAsync(request, cancellationToken);
                return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                var duration = DateTime.UtcNow - startTime;

                // Log response
                if (response != null)
                {
                    await LogResponse(response, requestId, duration);
                }
                else if (exception != null)
                {
                    LogException(exception, requestId, duration);
                }
            }
        }

        private async Task LogRequest(HttpRequestMessage request, string requestId)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"\n{'=',-80}");
            sb.AppendLine($"[{requestId}] HTTP REQUEST");
            sb.AppendLine($"{'=',-80}");
            sb.AppendLine($"{request.Method} {request.RequestUri}");

            // Log headers
            sb.AppendLine("\nHeaders:");
            foreach (var header in request.Headers)
            {
                // Mask Authorization header for security
                if (header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine($"  {header.Key}: [REDACTED]");
                }
                else
                {
                    sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                }
            }

            // Log request body if present
            if (request.Content != null)
            {
                var content = await request.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    sb.AppendLine("\nBody:");
                    // Redact sensitive payment information before logging
                    sb.AppendLine(SensitiveDataRedactor.RedactSensitiveData(content));
                }
            }

            Console.WriteLine(sb.ToString());
        }

        private async Task LogResponse(HttpResponseMessage response, string requestId, TimeSpan duration)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"\n{'-',-80}");
            sb.AppendLine($"[{requestId}] HTTP RESPONSE ({duration.TotalMilliseconds:F2}ms)");
            sb.AppendLine($"{'-',-80}");
            sb.AppendLine($"Status: {(int)response.StatusCode} {response.StatusCode}");

            // Log headers
            sb.AppendLine("\nHeaders:");
            foreach (var header in response.Headers)
            {
                sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
            }

            // Log content headers
            if (response.Content != null)
            {
                foreach (var header in response.Content.Headers)
                {
                    sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                }
            }

            // Log response body
            if (response.Content != null)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    sb.AppendLine("\nBody:");
                    // Redact sensitive payment information before logging
                    sb.AppendLine(SensitiveDataRedactor.RedactSensitiveData(content));
                }
            }

            sb.AppendLine($"{'=',-80}\n");
            Console.WriteLine(sb.ToString());
        }

        private void LogException(Exception exception, string requestId, TimeSpan duration)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"\n{'-',-80}");
            sb.AppendLine($"[{requestId}] HTTP EXCEPTION ({duration.TotalMilliseconds:F2}ms)");
            sb.AppendLine($"{'-',-80}");
            sb.AppendLine($"Exception: {exception.GetType().Name}");
            sb.AppendLine($"Message: {exception.Message}");
            sb.AppendLine($"{'=',-80}\n");
            Console.WriteLine(sb.ToString());
        }
    }
}
