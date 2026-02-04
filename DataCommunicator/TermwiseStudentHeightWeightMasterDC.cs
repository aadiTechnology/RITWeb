// -----------------------------------------------------------------------
// <copyright file="TermwiseStudentHeightWeightMasterDC.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudentEntities;

namespace DataCommunicator
{
        /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TermwiseStudentHeightWeightMasterDC
    {
        #region DATA MEMBER(S)

        public int miSchoolId;
        public int miAcademicYearId;

        #endregion

        #region CONSTRUCTOR(S)

        public TermwiseStudentHeightWeightMasterDC()
        {
            //Default constructor
        }

        public TermwiseStudentHeightWeightMasterDC(int aiSchoolId, int aiAcademicYearId)
        {
            miAcademicYearId = aiAcademicYearId;
            miSchoolId = aiSchoolId;
        }

        #endregion

        #region PUBLIC METHOD(S)

        /// <summary>
        /// This method is used to set fetched student height weight details set to an object
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        public List<StudentInfoForHeightWeight> GetStudentHeightWeightInfo(SqlDataReader aoSqlDataReader)
        {
            List<StudentInfoForHeightWeight> lstStudeentDetails = new List<StudentInfoForHeightWeight>();
            StudentInfoForHeightWeight oStudentInfo;
            if (aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    oStudentInfo = new StudentInfoForHeightWeight()
                    {
                        RollNo = Convert.ToInt32(aoSqlDataReader["Roll_No"]),
                        StudentName = aoSqlDataReader["StudentName"].ToString(),
                        Height = Convert.ToDecimal(aoSqlDataReader["Height"]),
                        Weight = Convert.ToDecimal(aoSqlDataReader["Weight"]),
                        IsLeftStudent = Convert.ToInt32(aoSqlDataReader["IsLeftStudent"]),

                        YearWiseStudentId = Convert.ToInt32(aoSqlDataReader["Student_Id"])
                    };
                    lstStudeentDetails.Add(oStudentInfo);
                }
            }
            return lstStudeentDetails;
        }

        /// <summary>
        /// This method is used to get all student height weight details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearID"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public List<StudentInfoForHeightWeight> GetStudentDetailsForHeightWeight(int aiSchoolId, int aiAcademicYearID, int aiStdDivId, int aiTermId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Term_Id", aiTermId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentListToCaptureHeighthWeight"))
                return GetStudentHeightWeightInfo(oSqlDataReader);
            };
        }

        /// <summary>
        /// This method is used to update height weight details.
        /// </summary>
        /// <param name="asStudentHeightWeight"></param>
        /// <param name="miSchoolId"></param>
        /// <param name="miAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiTermId"></param>
        public void UpdateStudentDetailsForHeightWeight(string asStudentHeightWeight, int miSchoolId, int miAcademicYearId, int aiUserId, int aiStdDivId, int aiTermId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentHeightWeightDetailsXML", asStudentHeightWeight, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateStudentDetailsForHeightWeight");
            }
        }

        #endregion
    }
}
