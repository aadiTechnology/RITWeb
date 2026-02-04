using System;

namespace SchoolEntities.Common
{
    /// <summary>
    /// Entity class to represent the result of a transaction operation.
    /// </summary>
    public class TransactionResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the transaction was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the error message if the transaction failed.
        /// </summary>
        public string Message { get; set; }
    }
}
