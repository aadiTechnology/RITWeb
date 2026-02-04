using System;

// Class Name       :- DuplicateEntityException
// Purpose          :- This class is used to through exception for duplicate entity which is through by Developer.
//                     We used this class like collection, which is used when we through duplicate record exception 
//                     and show error message to the client side mean (.aspx)
// Date Of creation :- 25/04/2009
// Author Name      :- Ashish

namespace BusinessLogic.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        private string sMessage = string.Empty;

        public string ErrorMessage
        {
            get
            {
                return sMessage;
            }
        }
        public DuplicateEntityException(string asMessage)
        {
            sMessage = asMessage;
        }
    }
}
