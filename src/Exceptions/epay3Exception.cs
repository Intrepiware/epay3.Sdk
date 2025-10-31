using System;

namespace epay3.Sdk.Exceptions
{
    /// <summary>
    /// Base exception for all epay3 SDK exceptions.
    /// </summary>
    public class epay3Exception : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="epay3Exception"/> class.
        /// </summary>
        public epay3Exception()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="epay3Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public epay3Exception(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="epay3Exception"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public epay3Exception(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
