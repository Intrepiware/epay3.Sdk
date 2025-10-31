using System;
using System.Net;

namespace epay3.Sdk.Exceptions
{
    /// <summary>
    /// Exception thrown when the API returns an error response.
    /// </summary>
    public class epay3ApiException : epay3Exception
    {
        /// <summary>
        /// Gets the HTTP status code of the error response.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the response body content.
        /// </summary>
        public string ResponseBody { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="epay3ApiException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="responseBody">The response body content.</param>
        public epay3ApiException(string message, HttpStatusCode statusCode, string responseBody) : base(message)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="epay3ApiException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="responseBody">The response body content.</param>
        /// <param name="innerException">The inner exception.</param>
        public epay3ApiException(string message, HttpStatusCode statusCode, string responseBody, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}
