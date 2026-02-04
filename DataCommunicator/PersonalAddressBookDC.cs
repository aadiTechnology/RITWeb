
// Class Name       :- PersonalAddressBookDC
// Purpose          :- This class is used to manage PersonalAddressBook details.
// Date Of creation :- 8/12/2009
// Author Name      :- Shankar 

using System;
using System.Data.SqlClient;
using System.Data;
using Utility;

namespace DataCommunicator
{
    public class PersonalAddressBookDC
    {
        private PersonalAddressBookStruct moPersonalAddressBookStruct;

        public PersonalAddressBookDC()
        {
        }

        public PersonalAddressBookDC(int miPersonalAddressBookId)
        {
            LoadPersonalAddressBookDetails(miPersonalAddressBookId);
        }

        public virtual PersonalAddressBookStruct PersonalAddressBookStructDetails
        {
            get
            {
                return moPersonalAddressBookStruct;
            }
            set
            {
                moPersonalAddressBookStruct = value;
            }
        }

        // This function is used to insert the PersonalAddressBook Details
        public int InsertPersonalAddressBook()
        {
            string sInsertStatement = "INSERT INTO PersonalAddressBook(" +
            "User_Id" +
            ",Name" +
            ",Mobile_No" +
            ",Is_Deleted" +
            ",Insert_Date" +
            ",Inserted_By_id" +
            ")VALUES(" +
            " " + moPersonalAddressBookStruct.miUserId +
             " , N'" + StringUtility.ReplaceSingleQuoteInString(moPersonalAddressBookStruct.msName, false) + "' " +
             " , N'" + StringUtility.ReplaceSingleQuoteInString(moPersonalAddressBookStruct.msMobileNo, false) + "' " +
             " , N'" + moPersonalAddressBookStruct.mblnIsDeleted + "' " +
             " , N'" + moPersonalAddressBookStruct.mdtInsertDate + "' " +
             " , " + moPersonalAddressBookStruct.miInsertedByid +
            ")";
            int iReturnValue = 0;
            using (SQLServerDbUtility oSQLServerUtility = new SQLServerDbUtility())
            {
                iReturnValue = oSQLServerUtility.ExecuteTransaction(sInsertStatement);
            }
            return iReturnValue;
        }

        // This function is used to update the PersonalAddressBook Details
        public void UpdatePersonalAddressBook()
        {
            string sUpdateStatement = "UPDATE PersonalAddressBook SET " +
            " Name= N'" + StringUtility.ReplaceSingleQuoteInString(moPersonalAddressBookStruct.msName, false) + "' " +
            ",Mobile_No= N'" + StringUtility.ReplaceSingleQuoteInString(moPersonalAddressBookStruct.msMobileNo, false) + "' " +
            ",Update_Date= N'" + moPersonalAddressBookStruct.mdtUpdateDate + "' " +
            ",Updated_By_Id= " + moPersonalAddressBookStruct.miUpdatedById +
            "" +
            " WHERE PersonalAddressBookId=" + moPersonalAddressBookStruct.miPersonalAddressBookId;
            using (SQLServerDbUtility oSQLServerUtility = new SQLServerDbUtility())
            {
                oSQLServerUtility.ExecuteTransaction(sUpdateStatement);
            }
        }

        // This function is used to delete the PersonalAddressBook Details
        public void DeletePersonalAddressBook()
        {
            string sUpdateStatement = "UPDATE PersonalAddressBook SET " +
            " Is_Deleted= N'" + moPersonalAddressBookStruct.mblnIsDeleted + "' " +
            ",Update_Date= N'" + moPersonalAddressBookStruct.mdtUpdateDate + "' " +
            ",Updated_By_Id= " + moPersonalAddressBookStruct.miUpdatedById +
            "" +
            " WHERE PersonalAddressBookId=" + moPersonalAddressBookStruct.miPersonalAddressBookId;
            using (SQLServerDbUtility oSQLServerUtility = new SQLServerDbUtility())
            {
                oSQLServerUtility.ExecuteTransaction(sUpdateStatement);
            }
        }

        // This function is used to load the PersonalAddressBook Details
        private void LoadPersonalAddressBookDetails(int miPersonalAddressBookId)
        {
            using (SQLServerDbUtility oSQLServerUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchPersonalAddressBookDetailsFromDatabase(miPersonalAddressBookId);
                using(SqlDataReader oDR = oSQLServerUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["PersonalAddressBookId"] != DBNull.Value)
                                moPersonalAddressBookStruct.miPersonalAddressBookId = Convert.ToInt32(oDR["PersonalAddressBookId"]);
                            if (oDR["User_Id"] != DBNull.Value)
                                moPersonalAddressBookStruct.miUserId = Convert.ToInt32(oDR["User_Id"]);
                            if (oDR["Name"] != DBNull.Value)
                                moPersonalAddressBookStruct.msName = Convert.ToString(oDR["Name"]);
                            if (oDR["Mobile_No"] != DBNull.Value)
                                moPersonalAddressBookStruct.msMobileNo = Convert.ToString(oDR["Mobile_No"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moPersonalAddressBookStruct.mblnIsDeleted = Convert.ToBoolean(oDR["Is_Deleted"]);
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moPersonalAddressBookStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moPersonalAddressBookStruct.miInsertedByid = Convert.ToInt32(oDR["Inserted_By_id"]);
                            if (oDR["Update_Date"] != DBNull.Value)
                                moPersonalAddressBookStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moPersonalAddressBookStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"]);
                        }
                    }
                }
            }
        }

        // This function is used to fetch the PersonalAddressBook Details
        private string FetchPersonalAddressBookDetailsFromDatabase(int miPersonalAddressBookId)
        {
            string sSelectStatement = " SELECT  " +
            "PersonalAddressBookId" +
            ",User_Id" +
            ",Name" +
            ",Mobile_No" +
            ",Is_Deleted" +
            ",Insert_Date" +
            ",Inserted_By_id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            " FROM PersonalAddressBook" +
            " WHERE PersonalAddressBookId=" + miPersonalAddressBookId +
            " AND  Is_Deleted=0";
            return sSelectStatement;
        }

        public struct PersonalAddressBookStruct
        {

            public int miPersonalAddressBookId;

            public int miUserId;

            public string msName;

            public string msMobileNo;

            public bool mblnIsDeleted;

            public DateTime mdtInsertDate;

            public int miInsertedByid;

            public DateTime mdtUpdateDate;

            public int miUpdatedById;
        }

        public DataTable GetAddressBookList(int aiUserId)
        {
            string sSelectStatement = " SELECT  " +
            "PersonalAddressBookId" +
            ",User_Id" +
            ",Name" +
            ",Mobile_No" +
            ",Is_Deleted" +
            ",Insert_Date" +
            ",Inserted_By_id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            " FROM PersonalAddressBook" +
            " WHERE User_Id=" + aiUserId +
            " AND  Is_Deleted=0";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
            }
        }

        public DataTable GetAddressBookGroupList(int aiUserId, string asGroupMob)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iUser_Id", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GroupMob", asGroupMob, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllGroupsDetails");
            }
        }

        public DataTable GetAddressBookGroupDetails(int aiUserId, int aiGroupID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iUser_Id", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iGroupId", aiGroupID, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetGroupDetails");
            }
        }

        public string CheckIfAlreadyExists()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iUser_Id", moPersonalAddressBookStruct.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iPersonalAddressBookId", moPersonalAddressBookStruct.miPersonalAddressBookId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sUser_Name", moPersonalAddressBookStruct.msName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sUser_Mobile_No", moPersonalAddressBookStruct.msMobileNo, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("sErrorMassage", null, SqlDbType.NVarChar, ParameterDirection.Output, 100);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsParsonalAddressExist");
                return oSqlParameter.Value.ToString();
            }
        }

        public string CheckIfGroupAlreadyExists(int aiPersonalBookGroupId, string asGroupName, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iUser_Id", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iPersonalAddressBookGroupId", aiPersonalBookGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GroupName", asGroupName, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("sErrorMassage", null, SqlDbType.NVarChar, ParameterDirection.Output, 100);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsParsonalAddressGroupExist");
                return oSqlParameter.Value.ToString();
            }
        }

        public void UpdatePersonalAddressBookGroup(int aiGroupID, string asGroupName, string asGroupDetailXML, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iGroupID", aiGroupID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sGroupName", asGroupName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sGroupDetailXML", asGroupDetailXML, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("iUserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdatePersonalAddressBookGroupDetails");
            }
        }

        public void InsertPersonalAddressBookGroup(string asGroupName, string asGroupDetailXML, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("sGroupName", asGroupName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sGroupDetailXML", asGroupDetailXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("iUserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertPersonalAddressBookGroupDetails");
            }
        }

        public DataTable GetDetailsOfGroups(string asGroupIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("sGroupIds", asGroupIds, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetDetailsOfGroups");
            }
        }

        public void DeletePersonalAddressBookGroup(int aiPersonalAddressBookGroupId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("PersonalAddressBookGroupId", aiPersonalAddressBookGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeletePersonalAddressBookGroup");
            }
        }
    }
}
