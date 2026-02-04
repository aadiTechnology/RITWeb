using System;
using System.Data;
using Utility;


namespace DataCommunicator
{
    public class UserAuthenticationDC
    {
        private string msLogin;
        private string msPassword;

        private struct TableDetails
        {
            public String sLoginField;
            public String sPasswordField;
            public String sTableName;
            public String sSelectField;
            public String sUserRoleId;
        }

        TableDetails[] moTableDetails = new TableDetails[4];
        Int32 miUserId = 0;

        public UserAuthenticationDC(string asLogin, string asPassword)
        {
            msLogin = asLogin;
            msPassword = asPassword;
            //miUserType = Convert.ToInt32(aiUserType);
            SetTableDetails();
        }

        //public UserAuthenticationDC(string asLogin, string asPassword, Constants.UserType aiUserType)
        //{
        //    msLogin = asLogin;
        //    msPassword = asPassword;
        //    // miUserType = Convert.ToInt32(aiUserType);
        //    SetTableDetails();
        //}

        public DataSet CheckIfUserIsValidAndGetUserId()
        {
            // This function will check if the specified buyer is valid or not. 

            string sSelectStatement;
            string sWhereClause = "";

            if (msPassword != "")
            {
                sWhereClause = " AND " + moTableDetails[miUserId].sPasswordField + " = '" + msPassword + "'";
            }

            sSelectStatement = " SELECT " +
                               moTableDetails[miUserId].sSelectField +
                               "," + moTableDetails[miUserId].sUserRoleId +
                " FROM " +
                    moTableDetails[miUserId].sTableName +
                " WHERE " +
                     moTableDetails[miUserId].sLoginField + " = '" + StringUtility.ReplaceSingleQuoteInString(msLogin, false) + "'" + sWhereClause;

            // " AND " + moTableDetails[miUserType].sTableName + ".is_deleted ='" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSelectStatement);

        }

        private void SetTableDetails()
        {
            // This method sets the structure for table details.
            moTableDetails[0].sLoginField = "User_Login";
            moTableDetails[0].sPasswordField = "User_Password";
            moTableDetails[0].sSelectField = "User_Id";
            moTableDetails[0].sUserRoleId = "User_Role_Id";
            moTableDetails[0].sTableName = "User_Master";

            //moTableDetails[2].sLoginField = "login_id";
            //moTableDetails[2].sPasswordField = "password";
            //moTableDetails[2].sSelectField = "admin_id";
            //moTableDetails[2].sTableName = "Admin_Master";
            //moTableDetails[2].sUserType = "login_id";
        }

    }
}
