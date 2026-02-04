// Class Name       :- ODDetailsDC
// Purpose          :- This class is used to manage staff members OD details.
// Date Of creation :- 13/01/2016
// Author Name      :- Dnyaneshwar Shinde

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SchoolEntities;
using PayrollEntities;
using Utility;
using System.Data;
namespace DataCommunicator
{
    public class ODDetailsDC : DataCommunicatorBaseDC
    {
        #region " Data Members "

        private int miSchoolId;
        private int miUserId;

        #endregion

        #region " Constructor "

        public ODDetailsDC() { }
        public ODDetailsDC(int aiSchoolId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUserId = aiUserId;
        }

        #endregion

        #region " Methods "

        /// <summary>
        /// This method is used to get all staff members OD details.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>
        public List<ODDetails> GetAllODDetails(int aiStaffGroupId, int aiUserId, int aiSchoolId, string asSortExpression, int aiStartIndex, int aiEndIndex, string asFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpr", " ORDER BY " + asSortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllODDetails"))
                    return FillODDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to get count of all staff members OD details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public int Count(int aiSchoolId, string asFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CountODDetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to get Particular staff members OD details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="SchoolId"></param>
        public ODDetails GetODDetail(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                ODDetails oODDetails;
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ODId", aiId, SqlDbType.Int);
                oODDetails = new ODDetails();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetODDetail"))
                    if (oSqlDataReader.Read())
                    {
                        oODDetails.ODId = Convert.ToInt32(oSqlDataReader["ODId"]);
                        oODDetails.Date = Convert.ToDateTime(oSqlDataReader["Date"]);
                        oODDetails.EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"]);
                        oODDetails.UserId = Convert.ToInt32(oSqlDataReader["UserId"]);
                        oODDetails.StaffGroupId = Convert.ToInt32(oSqlDataReader["StaffGroupsId"]);
                        oODDetails.UserName = Convert.ToString(oSqlDataReader["UserName"]);
                        oODDetails.Location = Convert.ToString(oSqlDataReader["Location"]);
                        oODDetails.Description = Convert.ToString(oSqlDataReader["Description"]);
                    }
                return oODDetails;
            }
        }

        /// <summary>
        /// This method is used to save OD details.
        /// </summary>
        /// <param name="oODDetails"></param>
        public void SaveODDetails(ODDetails aoODDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ODId", aoODDetails.ODId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", aoODDetails.Date, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EndDate", aoODDetails.EndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("UserId", aoODDetails.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Location", aoODDetails.Location, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Description", aoODDetails.Description, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveODDetails");
            }
        }

        /// <summary>
        /// This method is used to delete OD details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="miSchoolId"></param>
        /// <param name="UpdatedById"></param>
        public void DeleteODDetails(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ODId", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteODDetails");
            }
        }

        /// <summary>
        /// This method is used to Add results to fill OD details.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        private List<ODDetails> FillODDetails(SqlDataReader oSqlDataReader)
        {
            List<ODDetails> lstODDetails = new List<ODDetails>();
            while (oSqlDataReader.Read())
            {
                ODDetails oODDetails = new ODDetails();
                oODDetails.ODId = Convert.ToInt32(oSqlDataReader["ODId"]);
                oODDetails.Date = Convert.ToDateTime(oSqlDataReader["Date"]);
                oODDetails.EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"]);
                oODDetails.UserName = Convert.ToString(oSqlDataReader["UserName"]);
                oODDetails.UserId = Convert.ToInt32(oSqlDataReader["UserId"]);
                oODDetails.Location = Convert.ToString(oSqlDataReader["Location"]);

                lstODDetails.Add(oODDetails);
            }
            return lstODDetails;
        }

        /// <summary>
        /// This method is used to get staff members OD Dates.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="SchoolId"></param>
        public List<ODDateDetails> GetODDates(int aiUserId)
        {
            List<ODDateDetails> lstODDateDetails = new List<ODDateDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetODDateDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        ODDateDetails oODDateDetails = new ODDateDetails();
                        oODDateDetails.Date = Convert.ToDateTime(oSqlDataReader["Date"]);

                        lstODDateDetails.Add(oODDateDetails);
                    }
                    return lstODDateDetails;
                }
            }
        }

        /// <summary>
        /// This method is used to get user staff group And user id for Search.
        /// </summary>
        /// <param name="asName"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public UserDetailsForOD GetUserDetailsForOD(string asName, int aiSchoolId, int aiAcademicYearId)
        {
            UserDetailsForOD oUserDetailsForOD;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserName", asName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oUserDetailsForOD = new UserDetailsForOD();

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserDetailsForOD"))
                    if (oSqlDataReader.Read())
                    {
                        oUserDetailsForOD.StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupsId"]);
                        oUserDetailsForOD.UserId = Convert.ToInt32(oSqlDataReader["UserId"]);
                    }
                return oUserDetailsForOD;
            }
        }
        #endregion
    }
}
