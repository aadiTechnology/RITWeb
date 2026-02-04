// Class Name       :- SuperAdminDetailsDC
// Purpose          :- This class is used to manage super admin details.
// Date Of creation :- 8/17/2011
// Author Name      :- Vipul Jadhav

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SuperAdminEntities;
using Utility;
//using DataCommunicator.SuperAdmin;
using DataCommunicator;
using System.Data;


namespace DataCommunicator
{
    public class SuperAdminDetailsDC : DataCommunicatorBaseDC
    {
		#region "Data Members"

        public SuperAdminDetails moSuperAdminDetails;

        #endregion "Data Members"

        #region "Constructor"

        public SuperAdminDetailsDC()
        {
            moSuperAdminDetails = new SuperAdminDetails();
        }

        public SuperAdminDetailsDC(int aiSuperAdminDetailsId)
        {
            moSuperAdminDetails = new SuperAdminDetails();
            LoadSuperAdminDetails(aiSuperAdminDetailsId);
        }

        #endregion "Constructor"

        /// <summary>
        /// This method is used to insret super admin details.
        /// </summary>
        public void Insert()
        {
            string sInsertSuperAdminDetails = "INSERT INTO [dbo].[SuperAdminDetails]" +
                                              "([SalutationId]" +
                                              " ,[FirstName]" +
                                              " ,[MiddleName]" +
                                              " ,[LastName]" +
                                              " ,[MobileNumber]" +
                                              " ,[Is_Deleted]" +
                                              " ,[InsertDate]" +
                                              " ,[InsertedById])" +
                                              " VALUES" +
                                              " (" + moSuperAdminDetails.SalutationId +
                                              " ,N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.FirstName,false) +
                                              "' ,N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.MiddleName,true) +
                                              "' ,N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.LastName,true) +
                                              "' ," + moSuperAdminDetails.MobileNumber +
                                              " ,N'" + Constants.C_NO +
                                              "' ,N'" + DateTime.Now.ToShortDateString() +
                                              "' , " + moSuperAdminDetails.InsertedById + ");" +
                                              "INSERT INTO [dbo].[Super_Admin]" +
                                              " ([Login_Name]" +
                                              " ,[Password]" +
                                              " ,[Is_Deleted]" +
                                              " ,[Inserted_By_Id]" +
                                              " ,[Insert_Date]" +
                                              " ,[UserRoleId]" +
                                              " ,[SuperAdminDetailsId])" +
                                              " VALUES" +
                                              " (N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.UserName,false) +
                                              "', N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.Password, false) +
                                              "', N'" + moSuperAdminDetails.Is_Deleted +
                                              "', " + moSuperAdminDetails.InsertedById +
                                              ", N'" + DateTime.Now.ToShortDateString() +
                                              "', " + Convert.ToInt32(Constants.SuperAdminRoles.ManagementUser) + ", (SELECT SCOPE_IDENTITY()))";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sInsertSuperAdminDetails);
        }

        /// <summary>
        /// This method is used to load super admin details.
        /// </summary>
        /// <param name="miSuperAdminDetailsId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        private void LoadSuperAdminDetails(int aiSuperAdminDetailsId)
        {
         
            string sSelectStatement = " SELECT	SuperAdminDetails.SalutationId," +
		                              " SuperAdminDetails.FirstName," +
                                      " SuperAdminDetails.MiddleName," +
                                      " SuperAdminDetails.LastName," +
                                      " SuperAdminDetails.MobileNumber," +
                                      " SuperAdminDetails.SuperAdminDetailsId," +
                                      " Super_Admin.Login_Name," +
                                      " Super_Admin.Password," +
                                      " Super_Admin.User_Id" +
                                      " FROM	Super_Admin " +
		                              " INNER JOIN SuperAdminDetails" +
		                              " ON Super_Admin.SuperAdminDetailsId = SuperAdminDetails.SuperAdminDetailsId" +
                                      " WHERE	Super_Admin.UserRoleId = " + Convert.ToInt32(Constants.SuperAdminRoles.ManagementUser) +
		                              " AND Super_Admin.Is_Deleted = 0" +
		                              " AND SuperAdminDetails.Is_Deleted = N'" + Constants.C_NO + "'" +
                                      " AND SuperAdminDetails.SuperAdminDetailsId = " + aiSuperAdminDetailsId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
               using( SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
               {
                   if (oDR != null)
                   {
                       while (oDR.Read())
                       {
                           if (oDR["FirstName"] != DBNull.Value)
                               moSuperAdminDetails.FirstName = Convert.ToString(oDR["FirstName"]);
                           if (oDR["MiddleName"] != DBNull.Value)
                               moSuperAdminDetails.MiddleName = Convert.ToString(oDR["MiddleName"]);
                           if (oDR["LastName"] != DBNull.Value)
                               moSuperAdminDetails.LastName = Convert.ToString(oDR["LastName"]);
                           if (oDR["MobileNumber"] != DBNull.Value)
                               moSuperAdminDetails.MobileNumber = Convert.ToString(oDR["MobileNumber"]);
                           if (oDR["SuperAdminDetailsId"] != DBNull.Value)
                               moSuperAdminDetails.SuperAdminDetailsId = Convert.ToInt32(oDR["SuperAdminDetailsId"]);
                           if (oDR["Login_Name"] != DBNull.Value)
                               moSuperAdminDetails.UserName = Convert.ToString(oDR["Login_Name"]);
                           if (oDR["Password"] != DBNull.Value)
                               moSuperAdminDetails.Password = Convert.ToString(oDR["Password"]);
                           if (oDR["User_Id"] != DBNull.Value)
                               moSuperAdminDetails.UserId = Convert.ToInt32(oDR["User_Id"]);
                           if (oDR["SalutationId"] != DBNull.Value)
                               moSuperAdminDetails.SalutationId = Convert.ToInt32(oDR["SalutationId"]);
                       }
                   }   
                }
            }
        }

        /// <summary>
        /// This method is used to get super admin details.
        /// </summary>
        /// <returns></returns>
        public List<SuperAdminDetails> GetAll()
        {
            List<SuperAdminDetails> lstSuperAdminDetails = new List<SuperAdminDetails>();
           
            string sSelectStatement = " SELECT  Salutation_Master.Salutation_Name + " +
		                              " N' ' + SuperAdminDetails.FirstName + " +
                                      " CASE WHEN SuperAdminDetails.MiddleName = N''" +
                                      " THEN N''" +
                                      " ELSE N' ' + SuperAdminDetails.MiddleName + N'.' " +
                                      " END +" +
                                      " CASE WHEN SuperAdminDetails.LastName = N''" +
                                      " THEN N''" +
                                      " ELSE N' ' + SuperAdminDetails.LastName" +
                                      " END AS FullName," +
                                      " SuperAdminDetails.MobileNumber," +
                                      " SuperAdminDetails.SuperAdminDetailsId," +
                                      " Super_Admin.User_Id" +
                                      " FROM	Super_Admin INNER JOIN" +
                                      " SuperAdminDetails" +
                                      " ON Super_Admin.SuperAdminDetailsId = SuperAdminDetails.SuperAdminDetailsId" +
                                      " INNER JOIN Salutation_Master" +
                                      " ON Salutation_Master.Salutation_Id = SuperAdminDetails.SalutationId" +
                                      " AND SuperAdminDetails.Is_Deleted = N'N'" +
                                      " AND Super_Admin.Is_Deleted = 0" +
                                      " AND Super_Admin.UserRoleId = " + Convert.ToInt32(Constants.SuperAdminRoles.ManagementUser);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
               using( SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
               {
                   if (oDR != null)
                   {
                       while (oDR.Read())
                       {
                           SuperAdminDetails oSuperAdminDetails = new SuperAdminDetails();
                           if (oDR["FullName"] != DBNull.Value)
                               oSuperAdminDetails.FullName = Convert.ToString(oDR["FullName"]);
                           if (oDR["MobileNumber"] != DBNull.Value)
                               oSuperAdminDetails.MobileNumber = Convert.ToString(oDR["MobileNumber"]);
                           if (oDR["SuperAdminDetailsId"] != DBNull.Value)
                               oSuperAdminDetails.SuperAdminDetailsId = Convert.ToInt32(oDR["SuperAdminDetailsId"]);
                           if (oDR["User_Id"] != DBNull.Value)
                               oSuperAdminDetails.UserId = Convert.ToInt32(oDR["User_Id"]);
                           lstSuperAdminDetails.Add(oSuperAdminDetails);
                       }
                   }
                }
            }
            return lstSuperAdminDetails;
        }

        public bool IsDuplicate()
        {
            string sFilter = string.Empty;
            int iDuplicateUserNameCount = 0;
            if (moSuperAdminDetails.UserId != 0)
            {
                sFilter = "AND User_Id <> " + moSuperAdminDetails.UserId;
            }
            string sSelectStatement = " SELECT COUNT(User_Id) FROM " +
                             " Super_Admin " +
                             " WHERE Is_Deleted = 0 " +
                             " AND Login_Name = N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.UserName,false) + "'" + sFilter;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iDuplicateUserNameCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iDuplicateUserNameCount > 0;
        }

        /// <summary>
        /// This method is used to update super admin details.
        /// </summary>
        public void Update()
        {
            string sUpdateStatement = " UPDATE SuperAdminDetails" +
                                      " SET SuperAdminDetails.SalutationId = " + moSuperAdminDetails.SalutationId +
                                      ", SuperAdminDetails.FirstName = N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.FirstName,false) +
                                      "', SuperAdminDetails.MiddleName = N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.MiddleName,true) +
                                      "', SuperAdminDetails.LastName = N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.LastName,true) +
                                      "', SuperAdminDetails.MobileNumber = " + moSuperAdminDetails.MobileNumber +
                                      ", UpdateDate = N'" + DateTime.Now.ToShortDateString() +
                                      "', UpdatedById = " + moSuperAdminDetails.UpdatedById +
                                      " WHERE SuperAdminDetails.Is_Deleted = N'N'" +
                                      " AND SuperAdminDetails.SuperAdminDetailsId = " + moSuperAdminDetails.SuperAdminDetailsId +";" +
                                      " UPDATE Super_Admin " +
                                      " SET Super_Admin.Login_Name = N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.UserName,false) +
                                      "', Super_Admin.Password = N'" + StringUtility.ReplaceSingleQuoteInString(moSuperAdminDetails.Password, false) +
                                      "', Update_Date = N'" + DateTime.Now.ToShortDateString() +
                                      "', Update_By_Id = " + moSuperAdminDetails.UpdatedById +
                                      " WHERE Is_Deleted = 0" +
                                      " AND Super_Admin.SuperAdminDetailsId = " + moSuperAdminDetails.SuperAdminDetailsId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This method is used to delete super admin details.
        /// </summary>
        public void Delete()
        {
            string sDeleteStatement = " UPDATE SuperAdminDetails" +
                                      " SET Is_Deleted = N'" + Constants.C_YES +
                                      "', UpdateDate = N'" + DateTime.Now.ToShortDateString() +
                                      "', UpdatedById = " + moSuperAdminDetails.UpdatedById +
                                      " WHERE SuperAdminDetailsId = " + moSuperAdminDetails.SuperAdminDetailsId + ";" +
                                      " UPDATE Super_Admin " +
                                      " SET Is_Deleted = 1 " +
                                      ", Update_Date = N'" + DateTime.Now.ToShortDateString() +
                                      "', Update_By_Id = " + moSuperAdminDetails.UpdatedById +
                                      " WHERE Is_Deleted = 0" +
                                      " AND Super_Admin.SuperAdminDetailsId = " + moSuperAdminDetails.SuperAdminDetailsId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }
        /// <summary>
        /// This method is used for getting RTE/NONRTE Students.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="isRTE"></param>
        /// <param name="asSearchText"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>

        public List<Studentdetails> GetAllStudent(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, bool abIsRTEStudent, string asSearchText, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<Studentdetails> lstStudent = new List<Studentdetails>();
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardID", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Is_RTE_Student", abIsRTEStudent, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("asSearchText", asSearchText, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", aiEndIndex, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetRTEStudents"))
                {
                    while (oSqlDataReader.Read())
                    {
                        Studentdetails oStudentdetails = new Studentdetails();
                        oStudentdetails.StudentId = Convert.ToInt32(oSqlDataReader["StudentId"]);
                        oStudentdetails.EnrolmentNumber = Convert.ToString(oSqlDataReader["EnrolmentNumber"]);
                        oStudentdetails.StudentName = Convert.ToString(oSqlDataReader["StudentName"]);
                        lstStudent.Add(oStudentdetails);
                    }
                }
                return lstStudent;
            }
        }
        /// <summary>
        /// This method is used for Count purpose of the RTE/NONRTE List.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="isRTE"></param>
        /// <param name="asSearchText"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, bool abIsRTEStudent, string asSearchText)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardID", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Is_RTE_Student", abIsRTEStudent, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("asSearchText", asSearchText, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("CountRTENONRTEStudentdetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }
        /// <summary>
        /// This method is used for save records as RTE or NONRTE.
        /// </summary>
        /// <param name="asStudentId"></param>
        /// <param name="miSchoolId"></param>
        /// <param name="miAcademicYearId"></param>
        public void Save(string asStudentId, int aiSchoolId, int aiAcademicYearId, int aiUserId, bool abIsRTEStudent)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", asStudentId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Is_RTE_Student", abIsRTEStudent, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_SaveRTEStudents");
            }
        }
    }
}
