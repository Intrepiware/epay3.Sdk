using System;

namespace epay3.Sdk.Exceptions
{
    /// <summary>
    /// Exception thrown when authentication fails.
    /// </summary>
    public class AuthenticationException : epay3Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationException"/> class.
        /// </summary>
        public AuthenticationException() : base("Authentication failed. Please verify your API key and secret.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AuthenticationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationException"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public AuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}