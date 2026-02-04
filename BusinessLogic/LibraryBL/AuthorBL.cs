using DataCommunicator;

namespace BusinessLogic
{
    public class AuthorBL
    {
        #region Data members

       // private AuthorDC.AuthorStructDetails moAuthorStructDetails;
        private AuthorDC moAuthorDC = new AuthorDC();

        #endregion

        public AuthorBL()
        {
   //         moAuthorDC.AuthorInfo = moAuthorStructDetails;
        }
    }
}
