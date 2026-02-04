using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Utility;


namespace DataCommunicator
{
    public class LibraryConfigurationDC : DataCommunicatorBaseDC
    {
       public struct LibraryConfigurationStructDetails
        {
            public Int32 miSchoolId;
            public Int32 miAcademicYearId;
            public Int32 miUserRoleId;
            public Int32 miReturnDays;
            public Int32 miRenewAttempt;
            public Int32 miBookPerPerson;
            public Int32 miLateFeePerDay;
            public Int32 miLateFeeEffectiveDays;
            public Int32 miLibConfigId;
            public Int32 miReserveBooks;
            public Int32 miUser_Id;           
            public Int32 miInsertedById;
            public DateTime mdtInsertedDate;
            public Int32 miUpdatedById;
            public DateTime mdtUpdatedDate;
        }

        public LibraryConfigurationDC()
        {
        }

        public LibraryConfigurationDC(int aiUserRoleID, int aiSchoolId, int aiAcademicYearId)
        {
            LoadLibraryConfigDetails(aiUserRoleID, aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to load library setting details to respected feilds.
        /// </summary>
        /// <param name="aiUserRoleID"></param>
        /// <param name="aiSchoolId"></param>
        private void LoadLibraryConfigDetails(int aiUserRoleID, int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatement = " SELECT " +
                                            "User_Role_Id" +
                                            ",User_Role_Name" +
                                            ",Return_Days" +
                                            ",NoOf_Attempt_Renew" +
                                            ",No_Of_Book_Per_Person" +
                                            ",Late_Fee_Per_Day" +
                                            ",Late_Fee_Effective_From" +
                                            ",Lib_Config_Id" +
                                            ",Reserve_Books_Per_Person"+
                                         " FROM " +
                                            "vw_GetLibraryConfigDetails " +
                                         " WHERE " +
                                               " User_Role_Id=" + aiUserRoleID +
                                               " AND Is_Deleted=N'" + Constants.C_NO + "'" +
                                               " AND School_Id =" + aiSchoolId +
                                               " AND Academic_Year_Id=" + aiAcademicYearId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader DR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (DR.Read())
                    {
                        if (DR["User_Role_Id"] != DBNull.Value)
                            moLibraryConfigurationStructDetails.miUserRoleId = Convert.ToInt32(DR["User_Role_Id"]);
                        if (DR["Return_Days"] != DBNull.Value)
                            moLibraryConfigurationStructDetails.miReturnDays = Convert.ToInt32(DR["Return_Days"]);
                        if (DR["NoOf_Attempt_Renew"] != DBNull.Value)
                            moLibraryConfigurationStructDetails.miRenewAttempt = Convert.ToInt32(DR["NoOf_Attempt_Renew"]);
                        if (DR["No_Of_Book_Per_Person"] != DBNull.Value)
                            moLibraryConfigurationStructDetails.miBookPerPerson = Convert.ToInt32(DR["No_Of_Book_Per_Person"]);
                        if (DR["Late_Fee_Per_Day"] != DBNull.Value)
                            moLibraryConfigurationStructDetails.miLateFeePerDay = Convert.ToInt32(DR["Late_Fee_Per_Day"]);
                        if (DR["Late_Fee_Effective_From"] != DBNull.Value)
                            moLibraryConfigurationStructDetails.miLateFeeEffectiveDays = Convert.ToInt32(DR["Late_Fee_Effective_From"]);
                        if (DR["Lib_Config_Id"] != DBNull.Value)
                            moLibraryConfigurationStructDetails.miLibConfigId = Convert.ToInt32(DR["Lib_Config_Id"]);
                        if (DR["Reserve_Books_Per_Person"] != DBNull.Value)
                            moLibraryConfigurationStructDetails.miReserveBooks = Convert.ToInt32(DR["Reserve_Books_Per_Person"]);
                    }
                }
            }
        }

        private LibraryConfigurationStructDetails moLibraryConfigurationStructDetails;

        #region Property

        public LibraryConfigurationStructDetails LibraryConfigurationInfo
        {
            get
            {
                return moLibraryConfigurationStructDetails;
            }
            set
            {
                moLibraryConfigurationStructDetails = value;
            }

        }
        #endregion

        /// <summary>
        /// This method is used to retrive library setting details from view.
        /// </summary>
        /// <returns></returns>
        public DataTable RetriveLibraryConfigurarion()
        {

            string sSelectStatment = "SELECT " +
                                        "Lib_Config_Id" +
                                        ",User_Role_Name" +
                                        ",Return_Days" +
                                        ",NoOf_Attempt_Renew" +
                                        ",No_Of_Book_Per_Person" +
                                        ",Late_Fee_Per_Day" +
                                        ",Late_Fee_Effective_From" +
                                        ",Reserve_Books_Per_Person "+
                                     " FROM " +
                                        "vw_GetLibraryConfigDetails " +
                                     " WHERE " +
                                         " School_Id=" + moLibraryConfigurationStructDetails.miSchoolId +
                                         " AND Academic_Year_Id=" + moLibraryConfigurationStructDetails.miAcademicYearId +
                                         " AND Is_Deleted = 'N' " + 
                                     " ORDER BY User_Role_Id";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())      
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatment);
        }

        /// <summary>
        /// This method is used to add library setting to the database.
        /// </summary>
        public void AddLibraryConfigurarion()
        {
            string sInsertStatment = "INSERT INTO Library_Configuration_Master(" +
                                   " User_Role_Id" +
                                   ",Return_Days" +
                                   ",NoOf_Attempt_Renew" +
                                   ",No_Of_Book_Per_Person" +
                                   ",Late_Fee_Per_Day" +
                                   ",Late_Fee_Effective_From" +
                                   ",Reserve_Books_Per_Person"+
                                   ",School_Id" +
                                   ",Academic_Year_Id" +
                                   ",Inserted_By_id )" +
                               " VALUES (" +
                                   "" + moLibraryConfigurationStructDetails.miUserRoleId +
                                   "," + moLibraryConfigurationStructDetails.miReturnDays +
                                   "," + moLibraryConfigurationStructDetails.miRenewAttempt +
                                   "," + moLibraryConfigurationStructDetails.miBookPerPerson +
                                   "," + moLibraryConfigurationStructDetails.miLateFeePerDay +
                                   "," + moLibraryConfigurationStructDetails.miLateFeeEffectiveDays +
                                   ","+moLibraryConfigurationStructDetails.miReserveBooks+
                                   "," + moLibraryConfigurationStructDetails.miSchoolId +
                                   "," + moLibraryConfigurationStructDetails.miAcademicYearId +
                                   "," + moLibraryConfigurationStructDetails.miInsertedById+
                                   ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sInsertStatment);
        }

        /// <summary>
        /// This method is used to update(edit) library setting details to the database.
        /// </summary>
        public void UpdateLibraryConfigurarion()
        {
            string sUpdateStatment = "UPDATE Library_Configuration_Master SET" +
                                       " Return_Days=" + moLibraryConfigurationStructDetails.miReturnDays +
                                       " ,NoOf_Attempt_Renew=" + moLibraryConfigurationStructDetails.miRenewAttempt +
                                       " ,No_Of_Book_Per_Person=" + moLibraryConfigurationStructDetails.miBookPerPerson +
                                       " ,Late_Fee_Per_Day=" + moLibraryConfigurationStructDetails.miLateFeePerDay +
                                       " ,Late_Fee_Effective_From=" + moLibraryConfigurationStructDetails.miLateFeeEffectiveDays +  
                                       " ,Reserve_Books_Per_Person="+moLibraryConfigurationStructDetails.miReserveBooks+
                                       " ,Updated_By_Id="+moLibraryConfigurationStructDetails.miUpdatedById+
                                       " ,Update_Date=N'"+ moLibraryConfigurationStructDetails.mdtUpdatedDate.ToShortDateString() +"'"+
                                    " WHERE" +
                                        " School_Id=" + moLibraryConfigurationStructDetails.miSchoolId +
                                        " AND Academic_Year_Id=" + moLibraryConfigurationStructDetails.miAcademicYearId +
                                        " AND Lib_Config_Id=" + moLibraryConfigurationStructDetails.miLibConfigId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatment);
        }

        /// <summary>
        /// This method is used to get user role details to fill user role combo box.
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserRoles()
        {
            string sSelectStatement = "SELECT User_Role_Id " +
                                      ",User_Role_Name " +
                                      " FROM User_Role_Master " +
                                      " WHERE User_Role_Id IN (1,2,3,6,7,9)";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
    }
}
