using System;

namespace epay3.Sdk.Attributes
{
    /// <summary>
    /// Specifies how sensitive data should be redacted in logs.
    /// </summary>
    public enum RedactionMode
    {
        /// <summary>
        /// Completely redact the value (e.g., "XXX" for CVCs).
        /// </summary>
        Complete,

        /// <summary>
        /// Mask the value, showing only the last 4 characters (e.g., "****1111" for card numbers).
        /// </summary>
        MaskShowLast4,

        /// <summary>
        /// Mask the value, showing first 4 and last 4 characters (e.g., "1234****5678" for account numbers).
        /// </summary>
        MaskShowFirstAndLast4
    }

    /// <summary>
    /// Marks a property as containing sensitive data that should be redacted in logs.
    /// Apply this attribute to properties that contain payment information, personal data, or other sensitive values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SensitiveDataAttribute : Attribute
    {
        /// <summary>
        /// Gets the redaction mode for this sensitive property.
        /// </summary>
        public RedactionMode RedactionMode { get; }

        /// <summary>
        /// Gets or sets the replacement text for complete redaction.
        /// Default is "***REDACTED***" for complete redaction.
        /// </summary>
        public string RedactedValue { get; set; } = "***REDACTED***";

        /// <summary>
        /// Gets or sets the character to use for masking.
        /// Default is 'X'.
        /// </summary>
        public char MaskCharacter { get; set; } = 'X';

        /// <summary>
        /// Initializes a new instance of the <see cref="SensitiveDataAttribute"/> class.
        /// </summary>
        /// <param name="redactionMode">The redaction mode to use for this property.</param>
        public SensitiveDataAttribute(RedactionMode redactionMode = RedactionMode.Complete)
        {
            RedactionMode = redactionMode;
        }
    }
}
