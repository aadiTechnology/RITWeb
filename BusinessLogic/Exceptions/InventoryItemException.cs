using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Exceptions
{
    /// <summary>
    /// This exception is thrown when item details are wrong.
    /// </summary>
    public class InvalidItemDataException : Exception
    {
        private string msMessage = string.Empty;

        /// <summary>
        ///  Constructs a new exception with the specified detail message.
        /// </summary>
        /// <param name="asMessage"></param>
        public InvalidItemDataException(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }
    }
}
