// Class Name       :- StudentLCUploadDC
// Purpose          :- This class is used to manage StudentLCUpload details.
// Date Of creation :- 28/3/2019
// Author Name      :- Sachin Wagh

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using Utility;
using LCUploadEntities;

namespace DataCommunicator
{
    public class StudentLCUploadDC
    {

        #region "Data Members"

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        private int miUpdatedById = 0;

        #endregion "Data Members"

        #region "Constructors"

        public StudentLCUploadDC()
        {
        }

        public StudentLCUploadDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUpdatedById = aiUpdatedById;
        }

        #endregion "Constructors"

        #region "Public Methods"

        /// <summary>
        /// This methos is used to get the Student LC Upload details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asUserName"></param>
        /// <param name="abChkUserWithPhotoFlag"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<StudentLCDetails> GetStudentLCUpload(int aiSchoolId, int aiAcademicYearId, string asUserName, bool abChkUserWithLCFlag, int aiStandardId, int aiDivisionId, int iEndIndex, int startRowIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", Utility.StringUtility.ReplaceSingleQuoteInString(asUserName, true), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FilterWithLC", abChkUserWithLCFlag, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentLCDetails"))                
                    return SetStudentsDetails(oSqlDataReader);                
            }            
        }

        /// <summary>
        /// This methos is used to get the Student LC Download details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiTotalRows"></param>
        /// <returns></returns>
        public List<StudentLCDetails> GetStudentLCDownload(int aiSchoolId, int aiEndIndex, int aistartRowIndex, string asFilter, string asSortExpression)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);                                
                oSQLServerDbUtility.AddParameter("prm_StartIndex", aistartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpr", " ORDER BY " + asSortExpression, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentLCDownloadDetails"))                                    
                    return SetStudentDownloadDetails(oSqlDataReader);                
            }
        }

        /// <summary>
        /// This methos is used to fill the student detail list for upload.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<StudentLCDetails> SetStudentsDetails(SqlDataReader aoSqlDataReader)
        {
            List<StudentLCDetails> lstStudentLCDetails = new List<StudentLCDetails>();
            StudentLCDetails oStudentLCDetails;
            while (aoSqlDataReader.Read())
            {
                oStudentLCDetails = new StudentLCDetails()
                {
                    EnrollmentNo = Convert.ToString(aoSqlDataReader["EnrolmentNo"]),
                    StudentId = Convert.ToInt32(aoSqlDataReader["StudentId"]),
                    RollNo = Convert.ToInt32(aoSqlDataReader["StduentRoll"]),
                    StudentName = Convert.ToString(aoSqlDataReader["StudentName"]),
                    LCFilePath = Convert.ToString(aoSqlDataReader["LCFileName"]),
                    LCUploadStatus = Convert.ToInt32(aoSqlDataReader["LCUploadStatus"]),
                    TotalRows = Convert.ToInt32(aoSqlDataReader["TotalRows"])
                };
                lstStudentLCDetails.Add(oStudentLCDetails);
            }
            return lstStudentLCDetails;
        }

        /// <summary>
        /// This methos is used to fill the student detail list for download
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<StudentLCDetails> SetStudentDownloadDetails(SqlDataReader aoSqlDataReader)
        {
            List<StudentLCDetails> lstStudentLCDetails = new List<StudentLCDetails>();
            while (aoSqlDataReader.Read())
            {
                StudentLCDetails oStudentLCDetails = new StudentLCDetails()
                {
                    SrNo = Convert.ToInt32(aoSqlDataReader["RowID"]),
                    StudentId = Convert.ToInt32(aoSqlDataReader["SchoolWise_Student_Id"]),
                    LCNo = Convert.ToInt32(aoSqlDataReader["SrNo"]),
                    EnrollmentNo = Convert.ToString(aoSqlDataReader["Enrolment_Number"]),
                    StudentName = Convert.ToString(aoSqlDataReader["StudentName"]),
                    LCFilePath = Convert.ToString(aoSqlDataReader["LCFileName"]),
                    TotalRows = Convert.ToInt32(aoSqlDataReader["TotalRows"])
                };
                lstStudentLCDetails.Add(oStudentLCDetails);
            }

            return lstStudentLCDetails;
        }

        /// <summary>
        /// This method is used to upload student LC 
        /// </summary>
        /// <param name="aoArrayListInsertStatements"></param>
        public void UploadStudentLC(string asXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveLCFileDetails");
            }
        }

        /// <summary>
        /// This USP is used to delete uploaded students LC.
        /// </summary>
        /// <param name="asIds"></param>
        public void DeleteLCFiles(string asIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentIds", asIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeletedUploadedLCFIles");
            }
        }
        #endregion "Public Methods"
    }
}
