using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace epay3.Sdk.Models
{
    /// <summary>
    /// The interval by which payments should be run.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum PaymentInterval
    {
        /// <summary>Daily interval</summary>
        Day = 1,

        /// <summary>Weekly interval</summary>
        Week,

        /// <summary>Monthly interval</summary>
        Month,

        /// <summary>Yearly interval</summary>
        Year
    }
}
