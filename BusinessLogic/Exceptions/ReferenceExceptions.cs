using System;

namespace BusinessLogic.Exceptions
{
    public class ReferenceExceptions : Exception
    {
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        /// <summary>
        ///  Constructs a new exception with the specified detail message.
        /// </summary>
        /// <param name="asMessage"></param>
        public ReferenceExceptions(string asMessage)
            : base(asMessage)
        {
            //msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
        public ReferenceExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }
}
