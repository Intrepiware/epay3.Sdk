namespace epay3.Sdk.Models
{
    /// <summary>
    /// Response codes for refund and void operations.
    /// </summary>
    public enum ReversalResponseCode
    {
        /// <summary>Generic decline</summary>
        GenericDecline,

        /// <summary>Operation succeeded</summary>
        Success,

        /// <summary>Transaction was previously voided</summary>
        PreviouslyVoided,

        /// <summary>Transaction has already settled</summary>
        AlreadySettled,

        /// <summary>Transaction was previously rejected</summary>
        PreviouslyRejected,

        /// <summary>Transaction cannot be voided</summary>
        CannotBeVoided
    }
}
