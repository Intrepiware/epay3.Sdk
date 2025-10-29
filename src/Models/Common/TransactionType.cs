namespace epay3.Sdk.Models
{
    /// <summary>
    /// The type of transaction or payment method.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>ACH/eCheck transaction</summary>
        Ach,

        /// <summary>Visa credit card</summary>
        Visa,

        /// <summary>MasterCard credit card</summary>
        MasterCard,

        /// <summary>Discover credit card</summary>
        Discover,

        /// <summary>American Express credit card</summary>
        AmericanExpress,

        /// <summary>JCB credit card</summary>
        Jcb,

        /// <summary>Paper check</summary>
        PaperCheck,

        /// <summary>Lockbox check</summary>
        LockboxCheck
    }
}