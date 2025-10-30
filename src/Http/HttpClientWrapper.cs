using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using epay3.Sdk.Exceptions;
using epay3.Sdk.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace epay3.Sdk.Http
{
    /// <summary>
    /// Handles HTTP communication with the epay3 API.
    /// </summary>
    internal class HttpClientWrapper : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly epay3ClientOptions _options;
        private bool _disposed;

        /// <summary>
        /// JSON serialization settings with camelCase naming convention for API compatibility.
        /// </summary>
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(camelCaseText: true)
            }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper"/> class.
        /// </summary>
        /// <param name="options">The client configuration options.</param>
        public HttpClientWrapper(epay3ClientOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _options.Validate();

            // Create the HttpClient with optional logging handler
            if (_options.VerboseLogging)
            {
                var loggingHandler = new LoggingHandler(new HttpClientHandler());
                _httpClient = new HttpClient(loggingHandler);
            }
            else
            {
                _httpClient = new HttpClient();
            }

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);

            // Set up Basic Authentication
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.ApiKey}:{_options.ApiSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Sends a GET request to the specified endpoint.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <param name="path">The API endpoint path.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        public async Task<T> GetAsync<T>(string path, string impersonationAccountKey = null, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            AddImpersonationHeader(request, impersonationAccountKey);

            return await SendRequestAsync<T>(request, cancellationToken);
        }

        /// <summary>
        /// Sends a GET request with query parameters to the specified endpoint.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <param name="path">The API endpoint path.</param>
        /// <param name="queryParams">Query parameters as an anonymous object.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        public async Task<T> GetAsync<T>(string path, object queryParams, string impersonationAccountKey = null, CancellationToken cancellationToken = default)
        {
            var queryString = BuildQueryString(queryParams);
            var fullPath = string.IsNullOrEmpty(queryString) ? path : $"{path}?{queryString}";

            return await GetAsync<T>(fullPath, impersonationAccountKey, cancellationToken);
        }

        /// <summary>
        /// Sends a POST request to the specified endpoint.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <param name="path">The API endpoint path.</param>
        /// <param name="body">The request body.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The deserialized response.</returns>
        public async Task<T> PostAsync<T>(string path, object body, string impersonationAccountKey = null, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            AddImpersonationHeader(request, impersonationAccountKey);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body, JsonSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return await SendRequestAsync<T>(request, cancellationToken);
        }

        /// <summary>
        /// Sends a POST request to the specified endpoint and extracts the created resource ID from the Location header.
        /// </summary>
        /// <param name="path">The API endpoint path.</param>
        /// <param name="body">The request body.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The ID of the created resource.</returns>
        public async Task<long> PostAsync(string path, object body, string impersonationAccountKey = null, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            AddImpersonationHeader(request, impersonationAccountKey);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body, JsonSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await SendRequestAsync(request, cancellationToken);
            return ExtractIdFromLocationHeader(response);
        }

        /// <summary>
        /// Sends a POST request to the specified endpoint and returns the string ID from the Location header.
        /// </summary>
        /// <param name="path">The API endpoint path.</param>
        /// <param name="body">The request body.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The ID of the created resource as a string.</returns>
        public async Task<string> PostAsyncString(string path, object body, string impersonationAccountKey = null, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            AddImpersonationHeader(request, impersonationAccountKey);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body, JsonSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await SendRequestAsync(request, cancellationToken);
            return ExtractStringIdFromLocationHeader(response);
        }

        /// <summary>
        /// Sends a PUT request to the specified endpoint.
        /// </summary>
        /// <param name="path">The API endpoint path.</param>
        /// <param name="body">The request body.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task PutAsync(string path, object body, string impersonationAccountKey = null, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, path);
            AddImpersonationHeader(request, impersonationAccountKey);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body, JsonSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            await SendRequestAsync(request, cancellationToken);
        }

        /// <summary>
        /// Sends a DELETE request to the specified endpoint.
        /// </summary>
        /// <param name="path">The API endpoint path.</param>
        /// <param name="impersonationAccountKey">Optional impersonation account key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task DeleteAsync(string path, string impersonationAccountKey = null, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, path);
            AddImpersonationHeader(request, impersonationAccountKey);

            await SendRequestAsync(request, cancellationToken);
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new AuthenticationException("Invalid credentials. Please verify the key and secret used to generate the authorization header value.");
                }

                throw new epay3ApiException(
                    $"API request failed with status code {(int)response.StatusCode} ({response.StatusCode})",
                    response.StatusCode,
                    responseBody);
            }
            catch (Exception ex) when (!(ex is epay3Exception))
            {
                var responseBody = response != null ? await response.Content.ReadAsStringAsync() : null;
                throw new epay3Exception($"An error occurred while communicating with the API: {ex.Message}", ex);
            }
        }

        private async Task<T> SendRequestAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var response = await SendRequestAsync(request, cancellationToken))
            {
                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(content))
                {
                    // For reference types, return a new instance instead of null
                    // This handles 201 Created responses with empty bodies
                    var type = typeof(T);
                    if (!type.IsValueType || Nullable.GetUnderlyingType(type) != null)
                    {
                        var instance = Activator.CreateInstance<T>();

                        // If there's a Location header and the type has an Id property, populate it
                        if (response.Headers.Location != null)
                        {
                            var idProperty = type.GetProperty("Id");
                            if (idProperty != null && idProperty.CanWrite)
                            {
                                var locationPath = response.Headers.Location.ToString();
                                var segments = locationPath.Split('/');
                                var idString = segments.LastOrDefault();

                                if (idProperty.PropertyType == typeof(long) && long.TryParse(idString, out var longId))
                                {
                                    idProperty.SetValue(instance, longId);
                                }
                                else if (idProperty.PropertyType == typeof(string))
                                {
                                    idProperty.SetValue(instance, idString);
                                }
                            }
                        }

                        // If response is successful and type has response code property, set it to Success
                        if (response.IsSuccessStatusCode)
                        {
                            var paymentResponseCodeProperty = type.GetProperty("PaymentResponseCode");
                            if (paymentResponseCodeProperty != null && paymentResponseCodeProperty.CanWrite &&
                                paymentResponseCodeProperty.PropertyType == typeof(PaymentResponseCode))
                            {
                                paymentResponseCodeProperty.SetValue(instance, PaymentResponseCode.Success);
                            }

                            var reversalResponseCodeProperty = type.GetProperty("ReversalResponseCode");
                            if (reversalResponseCodeProperty != null && reversalResponseCodeProperty.CanWrite &&
                                reversalResponseCodeProperty.PropertyType == typeof(ReversalResponseCode))
                            {
                                reversalResponseCodeProperty.SetValue(instance, ReversalResponseCode.Success);
                            }
                        }

                        return instance;
                    }
                    return default(T);
                }

                try
                {
                    return JsonConvert.DeserializeObject<T>(content, JsonSettings);
                }
                catch (JsonException ex)
                {
                    throw new epay3Exception($"Failed to deserialize response: {ex.Message}", ex);
                }
            }
        }

        private void AddImpersonationHeader(HttpRequestMessage request, string impersonationAccountKey)
        {
            if (!string.IsNullOrWhiteSpace(impersonationAccountKey))
            {
                request.Headers.Add("impersonationAccountKey", impersonationAccountKey);
            }
        }

        private long ExtractIdFromLocationHeader(HttpResponseMessage response)
        {
            if (response.Headers.Location == null)
            {
                throw new epay3Exception("The response did not contain a Location header with the created resource ID.");
            }

            var locationPath = response.Headers.Location.ToString();
            var segments = locationPath.Split('/');
            var idString = segments.LastOrDefault();

            if (long.TryParse(idString, out var id))
            {
                return id;
            }

            throw new epay3Exception($"Could not parse ID from Location header: {locationPath}");
        }

        private string ExtractStringIdFromLocationHeader(HttpResponseMessage response)
        {
            if (response.Headers.Location == null)
            {
                throw new epay3Exception("The response did not contain a Location header with the created resource ID.");
            }

            var locationPath = response.Headers.Location.ToString();
            var segments = locationPath.Split('/');
            return segments.LastOrDefault();
        }

        private string BuildQueryString(object parameters)
        {
            if (parameters == null)
                return string.Empty;

            var properties = parameters.GetType().GetProperties();
            var queryParams = new List<string>();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(parameters);
                if (value != null)
                {
                    var valueString = value is DateTime dateTime
                        ? dateTime.ToString("O") // ISO 8601 format
                        : value.ToString();

                    queryParams.Add($"{prop.Name}={Uri.EscapeDataString(valueString)}");
                }
            }

            return string.Join("&", queryParams);
        }

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