using System;

namespace BusinessLogic
{

    public class SchoolUserAuthorization : UserAuthentication
    {
        public SchoolUserBL moBuyer;

        public SchoolUserAuthorization(string asLogin, String asPassword)
            : base(asLogin, asPassword)
        {
            if (miUserId != 0)
            {
                try
                {
                    moBuyer = new SchoolUserBL(asLogin, asPassword);
                }
                catch (BuyerNotFoundException ex)
                {
                    string sError = ex.Message;
                    throw new BuyerNotFoundException("Invalid password");
                }
            }
            else
            {
                throw new InvalidLoginException("Invalid Buyer login name.");
            }
        }
        public SchoolUserAuthorization(int aiSchoolId, string asLogin, String asPassword)
            : base(asLogin, asPassword)
        {
            if (miUserId != 0)
            {
                try
                {
                    moBuyer = new SchoolUserBL(aiSchoolId, asLogin, asPassword);
                }
                catch (BuyerNotFoundException ex)
                {
                    string sError = ex.Message;
                    throw new BuyerNotFoundException("Invalid password");
                }
            }
            else
            {
                throw new InvalidLoginException("Invalid Buyer login name.");
            }
        }
    }

}