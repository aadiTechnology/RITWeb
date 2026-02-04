using System;

namespace BusinessLogic.Exceptions
{

    /// <summary>
    /// This exception is thrown when bood details are wrong.
    /// </summary>
    public class InvalidBookDataException : Exception
    {
        private string msMessage = string.Empty;

        /// <summary>
        ///  Constructs a new exception with the specified detail message.
        /// </summary>
        /// <param name="asMessage"></param>
        public InvalidBookDataException(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }
    }    
}
