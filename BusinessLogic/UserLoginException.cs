using System;

namespace BusinessLogic
{
   
    // This exception is thrown when the password of the user is not correct. 
    public class InvalidLoginException : Exception
    {
        private string msMessage = "";

        public override string Message
        {
            get
            {
                return msMessage;
            }
        }

        public InvalidLoginException(string asMessage)
        {
            msMessage = asMessage;
        }

    }

    // This exception is thrown when the user login is not found.
    public class LoginNotFoundException : Exception
    {
        private string msMessage = "";

        public override string Message
        {
            get
            {
                return msMessage;
            }
        }

        public LoginNotFoundException(string asMessage)
        {
            msMessage = asMessage;
        }

    }

    public class DuplicateUserException : Exception
    {
        private string msMessage = "";

        public override string Message
        {
            get
            {
                return msMessage;
            }
        }

        public DuplicateUserException(string asMessage)
        {
            msMessage = asMessage;
        }

    }
}


