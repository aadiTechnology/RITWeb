using System;
using System.Data;
using DataCommunicator;

namespace BusinessLogic
{
    public class UserAuthentication
    {
        private string msLogin = "";
        private string msPassword = "";

        protected Int32 miUserId = 0;
        protected char miUserType;

        UserAuthenticationDC moUserAuthenticationDC = null;

        protected UserAuthentication(string asLogin, String asPassword)
        {
            msLogin = asLogin;
            msPassword = asPassword;
            DataSet oDSUser;

            moUserAuthenticationDC = new UserAuthenticationDC(msLogin, "");
            oDSUser = moUserAuthenticationDC.CheckIfUserIsValidAndGetUserId();
            
            if (oDSUser.Tables[0].Rows.Count != 0)
            {
                if (asPassword.ToString().Trim().Length != 0)
                    asPassword = Utility.CommonUtility.GetEncryptedPassword(asLogin, asPassword);

                moUserAuthenticationDC = new UserAuthenticationDC(msLogin, asPassword);
                oDSUser = moUserAuthenticationDC.CheckIfUserIsValidAndGetUserId();
                miUserId = Convert.ToInt32(oDSUser.Tables[0].Rows[0][0]);
                
                if (miUserId == 0)
                    throw new InvalidLoginException("Invalid password");
            }
            else
                throw new LoginNotFoundException("Invalid login name.");            

        }

        //protected UserAuthentication(string asLogin, String asPassword, Constants.UserType aoUserType)
        //{
        //    msLogin = asLogin;
        //    msPassword = asPassword;
        //    DataSet oDSUser;
        //    moUserAuthenticationDC = new UserAuthenticationDC(msLogin, "", aoUserType);
        //    oDSUser = moUserAuthenticationDC.CheckIfUserIsValidAndGetUserId();
        //    miUserId = Convert.ToInt32(oDSUser.Tables[0].Rows[0][0]);
        //    if (miUserId != 0)
        //    {
        //        moUserAuthenticationDC = new UserAuthenticationDC(msLogin, asPassword, aoUserType);
        //        oDSUser = moUserAuthenticationDC.CheckIfUserIsValidAndGetUserId();
        //        miUserId = Convert.ToInt32(oDSUser.Tables[0].Rows[0][0]);
        //        if (miUserId == 0)
        //            throw new InvalidLoginException("Invalid password");
        //    }
        //    else
        //        throw new LoginNotFoundException("Invalid login name.");

        //}
      
    }

}