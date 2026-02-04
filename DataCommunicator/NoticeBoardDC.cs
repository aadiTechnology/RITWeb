
// Class Name       :- NoticeBoardDC
// Purpose          :- This class is used to manage Notice Board details.
// Date Of creation :- 21/11/2008
// Author Name      :- Ashish


using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class NoticeBoardDC
    {
        private NoticeBoardStructDetails moNoticeBoardStructDetails;
        public NoticeBoardStructDetails NoticeBoardInfo
        {
            get
            {
                return moNoticeBoardStructDetails;
            }
            set
            {
                moNoticeBoardStructDetails = value;
            }
        }

        public struct NoticeBoardStructDetails
        {
            public int miSchoolId;

            public int miAcademicYearId;

            public string msNoticeMessage;

            public int miMessageId;

            public System.DateTime mdtStartDate;

            public System.DateTime mdtEndDate;

            public System.DateTime mdtInsertDate;

            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

            public List<int> oSelectedRoles;

        }

        /// <summary>
        /// This method is used to ger data table to fill notice board grid veiw.
        /// </summary>
        /// <returns></returns>       
        public DataTable RetriveRolesFromUserRoleMaster()
        {
            string SSelectStatement = "SELECT " +
                                    "User_Role_Id" +
                                    ",User_Role_Name" +
                                    " FROM " +
                                    "User_Role_Master" +
                                    " WHERE " +
                                    "Is_Deleted='N'"+
                                    " AND User_Role_Id<8 ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(SSelectStatement);
        }

        /// <summary>
        /// This method is used to add new notice board message in database.
        /// </summary>
        public void AddNoticeMessage()
        {

            string sInsertStatement = "INSERT INTO Notice_Board  ( " +
                                            " [Message]" +
                                            " , [Start_Date]" +
                                            " , [End_Date]" +
                                            " , [School_Id]" +
                                            " , [Academic_Year_Id]" +
                                            " , [Inserted_By_id]" +
                                        ") VALUES (" +
                                             "  N'" + StringUtility.ReplaceSingleQuoteInString(moNoticeBoardStructDetails.msNoticeMessage, false) + "' " +
                                             " , N'" + moNoticeBoardStructDetails.mdtStartDate.ToShortDateString() + "'" +
                                             " , N'" + moNoticeBoardStructDetails.mdtEndDate.ToShortDateString() + "'" +
                                             " , " + moNoticeBoardStructDetails.miSchoolId +
                                             " , " + moNoticeBoardStructDetails.miAcademicYearId +
                                             " , " + moNoticeBoardStructDetails.miInsertedById +
                                        " ) ";


            string[] sTransactionStatement = new string[3];
            sTransactionStatement[0] = sInsertStatement;
            sTransactionStatement[1] = DataCommunicatorBaseDC.GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY);
            sTransactionStatement[2] = InsertRoles(Constants.S_LAST_INSERTED_P_KEY);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sTransactionStatement);
        }

        /// <summary>
        /// This method is used to update notice board message in database table.
        /// </summary>
        public void UpdateNoticeMessage()
        {
            string sUpdateStatement = "UPDATE Notice_Board SET " +
                                            " [Message]=N'" + StringUtility.ReplaceSingleQuoteInString(moNoticeBoardStructDetails.msNoticeMessage, false) + "' " +
                                            " , [Start_Date]=N'" + moNoticeBoardStructDetails.mdtStartDate.ToShortDateString() + "'" +
                                            " , [End_Date]=N'" + moNoticeBoardStructDetails.mdtEndDate.ToShortDateString() + "'" +
                                            " , [Updated_By_Id]=" + moNoticeBoardStructDetails.miUpdatedById +
                                            " , [Update_Date]=N'" + moNoticeBoardStructDetails.mdtUpdateDate.ToShortDateString() + "'" +
                                         "  WHERE " +
                                            " [Message_Id]=" + moNoticeBoardStructDetails.miMessageId +
                                            " AND [School_Id]= " + moNoticeBoardStructDetails.miSchoolId +
                                            " AND [Academic_Year_Id]= " + moNoticeBoardStructDetails.miAcademicYearId;

            string[] sTransactionStatement = new string[3];
            sTransactionStatement[0] = sUpdateStatement;
            sTransactionStatement[1] = DeleteRoles();
            sTransactionStatement[2] = InsertRoles(moNoticeBoardStructDetails.miMessageId.ToString());
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sTransactionStatement);

        }

        /// <summary>
        /// This method is used to delete notice board message from database table.
        /// </summary>
        public void DeleteNoticeMessage()
        {
            string sDeleteStatement = "UPDATE Notice_Board SET " +
                                            " [Is_Deleted]=" + Constants.I_ONE +
                                            " , [Updated_By_Id]=" + moNoticeBoardStructDetails.miUpdatedById +
                                            " , [Update_Date]=N'" + moNoticeBoardStructDetails.mdtUpdateDate.ToShortDateString() + "'" +
                                         "  WHERE " +
                                            " [Message_Id]=" + moNoticeBoardStructDetails.miMessageId +
                                            " AND [School_Id]= " + moNoticeBoardStructDetails.miSchoolId +
                                            " AND [Academic_Year_Id]= " + moNoticeBoardStructDetails.miAcademicYearId;

            string[] sTransactionStatement = new string[2];
            sTransactionStatement[0] = sDeleteStatement;
            sTransactionStatement[1] = DeleteRoles();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sTransactionStatement);
        }

        /// <summary>
        /// This method is used to update default notice dates according to academic year date change.
        /// </summary>
        public void UpdateDefaultNoticeDates()
        {

            string sUpdateStatement = "UPDATE Notice_Board SET " +
                                          "[Start_Date]=N'" + moNoticeBoardStructDetails.mdtStartDate.ToString("MM/dd/yyyy") + "'" +
                                          " , [End_Date]=N'" + moNoticeBoardStructDetails.mdtEndDate.ToString("MM/dd/yyyy") + "'" +
                                          " , [Updated_By_Id]=" + moNoticeBoardStructDetails.miUpdatedById +
                                          " , [Update_Date]=N'" + moNoticeBoardStructDetails.mdtUpdateDate.ToString("MM/dd/yyyy") + "'" +
                                       "  WHERE " +
                                          "Is_Default_Msg=" + Constants.I_ONE +
                                          " AND [School_Id]= " + moNoticeBoardStructDetails.miSchoolId +
                                          " AND [Academic_Year_Id]= " + moNoticeBoardStructDetails.miAcademicYearId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

        }



        /// <summary>
        /// This method is used to insert notice board roles.
        /// </summary>
        /// <param name="MessageId"></param>
        /// <returns></returns>
        private string InsertRoles(string MessageId)
        {
            string sRoleInsertStatement = string.Empty;

            foreach (int iRoleValue in moNoticeBoardStructDetails.oSelectedRoles)
            {
                sRoleInsertStatement = sRoleInsertStatement + "INSERT INTO Notice_Board_Roles (" +
                                                                                "[Message_Id]" +
                                                                                ",[User_Role_Id]" +
                                                                                ",[Insert_Date]" +
                                                                                ",[Inserted_By_Id]" +
                                                                                ")VALUES(" +
                                                                                MessageId +
                                                                                "," + iRoleValue +
                                                                                ",N'" + DateTime.Today.ToShortDateString() + "'" +
                                                                                "," + moNoticeBoardStructDetails.miInsertedById +
                                                                                ");";
            }
            return sRoleInsertStatement;
        }
        /// <summary>
        /// This method is used to delete all the selected roles.
        /// </summary>
        /// <returns></returns>
        private string DeleteRoles()
        {
            string sRoleDeleteStatement = "DELETE FROM Notice_Board_Roles" +
                                            " WHERE " +
                                            "Message_Id=" + moNoticeBoardStructDetails.miMessageId;
            return sRoleDeleteStatement;
        }
        /// <summary>
        /// This method is used to retrive roles from notice board roles.
        /// </summary>
        /// <param name="IMessageId"></param>
        /// <returns></returns>
        public DataTable RetrieveRolesFromNoticeBoardRoles(int IMessageId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iMessageId", IMessageId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetNoticeBoardRoles");
            }
        }
    }
    public class NoticeBoardCollectionDC
    {
        public NoticeBoardCollectionDC()
        {
        }
        /// <summary>
        /// This method is used to get notice board 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAccYrId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public DataTable GetNoticeBoardDetails(int aiSchoolId, int aiAccYrId, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (!string.IsNullOrEmpty(sortExpression))
            {
                if (sortExpression == "Start_Date")
                    sortExpression = " ORDER BY " + sortExpression + " DESC";
                else
                    sortExpression = " ORDER BY " + sortExpression;
            }
            else
                sortExpression = " ORDER BY Start_Date DESC";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAccYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PageSize", maximumRows, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetNoticeBoardDetails");
            }
        }
        /// <summary>
        /// This method is used to calculate record count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAccYrId"></param>
        /// <returns></returns>
        public int CountNoticeBoardDetails(int aiSchoolId, int aiAccYrId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAccYrId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountNoticeForNoticeBoard");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }
    }
}
