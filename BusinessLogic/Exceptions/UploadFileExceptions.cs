using System;

namespace BusinessLogic.Exceptions
{
     public class UploadFileExceptions : Exception
    {
        private string msMessage = " ";
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
        public UploadFileExceptions(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="asMessage"></param>
        /// <param name="ex"></param>
         public UploadFileExceptions(string asMessage, Exception innerException)
            : base(asMessage, innerException)
        {
            //msMessage = asMessage;
        }

    }
}
