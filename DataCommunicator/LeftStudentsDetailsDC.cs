// Class Name       :- LeftStudentsDetailsDC
// Purpose          :- This class is used to manage academic yearwise left student details.
// Date Of creation :- 8/10/2015
// Author Name      :- Yogesh

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.AcademicYearwiseLeftStudentDetailsMaster;
using Utility;

namespace DataCommunicator
{
    public class LeftStudentsDetailsDC
    {
        #region PUBLIC METHOD(S)
        
        /// <summary>
        /// This method is used to get academic yearwise left student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asNameFilter"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <param name="aiEndRowIndex"></param>
        /// <returns></returns>
       
        public static List<AcademicYearwiseLeftStudentDetails> Get(int aiSchoolId, int aiAcademicYearId, int aiStandardId, string asNameFilter, string asSortDirection, int aiStartRowIndex, int aiEndRowIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NameFilter", asNameFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortDirection ", asSortDirection, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartRowIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndRowIndex", aiEndRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAcademicYearwiseLeftStudentsDetails"))
                    return LoadLeftStudentDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to count record for grid paging.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asNameFilter"></param>
        /// <returns></returns>
        public static int GetCount(int aiSchoolId, int aiAcademicYearId,int aiStandardId, string asNameFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NameFilter", asNameFilter, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetCountAcademicYearwiseLeftStudentsDetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to get Mobile number of student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public DataTable GetMobileNumber(string asStudentIds)
        {
            string query = "select User_Id,Mobile_Number,Mobile_Number2 ,Salutation_Name +' '+ First_Name +' '+ Middle_Name +' '+ Last_Name as Name from vw_BaseStudentDetails WITH(NOLOCK) where SchoolWise_Student_Id IN (" + asStudentIds + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
               return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(query);
        }
        /// <summary>
        /// This method is regarding about Readmission process for left students.
        /// </summary>
       

        public DataTable ReadmissionLeftStudent(int aiSchoolId, int aiAcademicYearId, string aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiStudentId, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_ReadmissionofLeftstudents");
            }
        }

        /// <summary>
        /// This method is used for to get data for fillong standard combobox.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable GetDataForStandardCombo(int aiSchoolId, int aiAcademicYearId)
        {
            string sSqlStatement = "SELECT Original_Standard_Id,Standard_Name FROM Standard_Master WITH(NOLOCK) WHERE School_Id = " + aiSchoolId + "AND academic_Year_Id=" + aiAcademicYearId + " AND Is_Deleted='N'";	
            using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSqlStatement);
        }

        #endregion

        #region PRIVATE METHOD(S)

        /// <summary>
        /// This method is used to add values into entity class.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private static List<AcademicYearwiseLeftStudentDetails> LoadLeftStudentDetails(SqlDataReader aoSqlDataReader)
        {
            List<AcademicYearwiseLeftStudentDetails> lstStudentDetails = new List<AcademicYearwiseLeftStudentDetails>();
            AcademicYearwiseLeftStudentDetails oAcademicYearwiseLeftStudentDetails;
            while (aoSqlDataReader.Read())
            {
                oAcademicYearwiseLeftStudentDetails = new AcademicYearwiseLeftStudentDetails()
                {
                     AcademicYearId = Convert.ToInt32(aoSqlDataReader["AcademicYearId"]),
                     ClassName = Convert.ToString(aoSqlDataReader["ClassName"]),
                     Name = Convert.ToString(aoSqlDataReader["Name"]),
                     RegNo = Convert.ToString(aoSqlDataReader["RegNo"]),
                     SchoolLeftDate = (Convert.ToDateTime(aoSqlDataReader["SchoolLeftDate"])).ToString(Constants.S_DATE_FORMAT),
                     StudentId = Convert.ToInt32(aoSqlDataReader["StudentId"]),
                     StandardId = Convert.ToInt32(aoSqlDataReader["StandardId"]),
                     DivisionId = Convert.ToInt32(aoSqlDataReader["DivisionId"]),
                     YearValue = Convert.ToString(aoSqlDataReader["YearValue"]),
                     TotalRowCount = Convert.ToInt32(aoSqlDataReader["TotalRowCount"])
                };
                lstStudentDetails.Add(oAcademicYearwiseLeftStudentDetails);
            }
            return lstStudentDetails;
        }

        #endregion
    }
}
