
// Class Name       :- NoticeBoardDC
// Purpose          :- This class is used for company admin login perpose.
// Date Of creation :- 05/12/2008
// Author Name      :- Ashish


using System.Data;
using Utility;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using SuperAdminEntities;

namespace DataCommunicator
{
    public class SuperAdminDC
    {
        public string sSuperAdminPass;        

        public SuperAdminDC()
        {
        }
        
        public string SuperAdminPass
        {
            get { return sSuperAdminPass; }
            set { sSuperAdminPass = value; }
        }

        private SuperAdminStructDetails moSuperAdminStructDetails;
        public SuperAdminStructDetails SuperAdminInfo
        {
            get
            {
                return moSuperAdminStructDetails;
            }
            set
            {
                moSuperAdminStructDetails = value;
            }
        }

        public struct SuperAdminStructDetails
        {
            public int miSuperAdminId;

            public string msSuperAdminName;
            
            public string msMobileNo;

            public System.DateTime mdtInsertDate;

            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

        }
        /// <summary>
        /// This method is used to get Admin notice to display into control panel.
        /// </summary>
        /// <param name="aiAdminUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataSet GetAdminNoticeForControlPanel(int aiAdminUserId, int aiSchoolId, int aiAcademicYrId)
        {
            string sUSPName = "usp_GetNoticeBoardMessageAccordingToRoles";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_User_Id", aiAdminUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TableName", "User_Master", SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Condition", "User_Id", SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet(sUSPName);
            }
        }

		/// <summary>
		/// This method is used to get user details for login
		/// </summary>
		/// <param name="asLogin"></param>
		/// <param name="asPassword"></param>
		/// <param name="abIncludeSuperAdmin"></param>
		/// <returns></returns>
        public static DataTable GetValidUserDetails(string asLogin, string asPassword, bool abIncludeSuperAdmin)
        {
            string sSqlStatement = String.Format(" select top 1 SA.User_Id, convert(varchar(20), dbo.GetLocalDate(DEFAULT), 113) as LoginDate, SA.UserRoleId, ISNULL(SM.Salutation_Name, '') + ' ' + ISNULL(SAD.FirstName, '') [Name],SA.SuperAdminDetailsId" +
												 "   from dbo.Super_Admin SA left join dbo.SuperAdminDetails SAD inner join dbo.Salutation_Master SM" +
												 "	   on SAD.SalutationId = SM.Salutation_Id" +
												 "	   on SA.SuperAdminDetailsId = SAD.SuperAdminDetailsId and SAD.Is_Deleted = 'N'" +
												 "  where SA.Is_Deleted = 0 and Login_Name = '{0}' and Password = '{1}' {2}",
												  Utility.StringUtility.ReplaceSingleQuoteInString(asLogin, true),
												  Utility.StringUtility.ReplaceSingleQuoteInString(asPassword, true),
												  abIncludeSuperAdmin ? String.Empty : String.Format("and SA.UserRoleId = {0}", Constants.SuperAdminRoles.ManagementUser.ToInt()));
											using (var oSQLServerDbUtility = new SQLServerDbUtility())
												return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSqlStatement);
        }

		/// <summary>
		/// This method is used toget login name
		/// </summary>
		/// <param name="aiUserId"></param>
		/// <returns></returns>
        public string GetLoginName(int aiUserId)
        {
            string sLoginName = string.Empty;
            string sSelectStatement = "SELECT TOP 1 Login_Name,Password " +
                                    " FROM " +
                                    "Super_Admin" +
                                    " WHERE " +
                                    "User_Id=" + aiUserId +
                                    " AND Is_Deleted = " + Constants.I_ZERO;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oSqlDataReader.Read())
                    {
                        sLoginName = oSqlDataReader["Login_Name"].ToString();
                        SuperAdminPass = CommonUtility.GetDecryptedPassword(sLoginName.ToLower(), oSqlDataReader["Password"].ToString());
                    }
                }
            }
            return sLoginName;
        }

        /// <summary>
        /// This method is used to get school admin user login name for proper login to the school.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static string GetSchoolAdminLoginName(int aiSchoolId)
        {
            string sSelectStatement = "SELECT  TOP 1   User_Login" +
                                            " FROM " +
                                            " User_Master " +
                                            " WHERE " +
                                            " User_Role_Id = " + 1 +
                                            " AND IsSuperAdmin=0 "+
                                            " AND School_Id = " + aiSchoolId +
                                            " AND Is_Deleted = N'" + Constants.C_NO + "' " +
                                            " AND Is_Locked = N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStatement);
        }

        public static void UpdateSuperAdminDetails(int aiUserId, string asPassword)
        {
            string sUpdateStatement = "UPDATE Super_Admin " +
                                    " SET " +
                                    "Password='" + asPassword + "'" +
                                    " WHERE " +
                                    "User_Id=" + aiUserId +
                                    " AND Is_Deleted=" + Constants.I_ZERO;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public static void Reset(int aiSchoolId, int aiAcademicYrId, char acResetSubjectTeacher, char acResetClassTeacher)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ResetSubjectTeacher", acResetSubjectTeacher, SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("ResetClassTeacher", acResetClassTeacher, SqlDbType.Char);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ResetClassSubTeacherAssignment");
            }
        }

        public static SuperAdminDetails GetSuperAdminSessionDetails(int aiSchoolId, string asLoginName)
        {
            SuperAdminDetails oSuperAdminDetails = null;
            string sSelectSatement = "SELECT TOP 1 User_Id,UserRoleId," +
                                     "(SELECT Academic_Year_ID " +
                                     " FROM	SchoolWise_Academic_Year_Master" +
                                     " WHERE SchoolWise_Academic_Year_Master.Is_Current_Year = N'" + Constants.C_YES + "'" +
                                     " AND SchoolWise_Academic_Year_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                     " AND School_Id ="+ aiSchoolId +") AS year_id  ,"+
                                     " (  SELECT FinancialYearId"+  
                                     " FROM Accounts.FinancialYearMaster "+
                                     " WHERE Accounts.FinancialYearMaster.IsCurrent=1"+
                                     " AND Accounts.FinancialYearMaster.IsDeleted=0 "+
                                     " AND Accounts.FinancialYearMaster.SchoolId="+aiSchoolId+") AS FinancialYearId"+
                                     " FROM Super_Admin" +
                                     " WHERE Login_Name = N'" + StringUtility.ReplaceSingleQuoteInString(asLoginName,false) + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectSatement))
                {
                    while (oSqlDataReader.Read())
                    {
                        oSuperAdminDetails = new SuperAdminDetails()
                        {
                            AcademicYearId = Convert.ToInt32(oSqlDataReader["year_id"]),
                            UserRoleId = Convert.ToInt32(oSqlDataReader["UserRoleId"]),
                            UserId = Convert.ToInt32(oSqlDataReader["User_Id"]),
                            FinancialYearId = Convert.ToInt32(oSqlDataReader["FinancialYearId"])
                        };
                    }
                }
            }

            return oSuperAdminDetails;
        }

        public void PublishAllExams(int aiSchoolId, int aiAcademicYearId, int aiPublishById,string asReason)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PublishById", aiPublishById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UnPublishReason",asReason,SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PublishAllExams");
            }
        }
    }
}