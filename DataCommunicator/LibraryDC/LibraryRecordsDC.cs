using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class LibraryRecordsDC
    {
        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;

        public LibraryRecordsDC()
        {
        }

        public LibraryRecordsDC(int aiSchoolId,int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
            this.miAcademicYearId = aiAcademicYearId;
        }


        public List<LibraryRecord> GetAll(int aiStandardId, int aiDivisionId, DateTime dtShowDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<LibraryRecord> lstLibraryRecord = new List<LibraryRecord>();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowDate", dtShowDate, SqlDbType.DateTime);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentLibraryDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstLibraryRecord.Add
                            (
                                new LibraryRecord
                                {
                                    BookNo = oSqlDataReader["Book_No"].ToString(),
                                    Comment = oSqlDataReader["Comment"].ToString(),
                                    StudentName = oSqlDataReader["StudentName"].ToString(),
                                    Id = oSqlDataReader["Id"].ToInt(),
                                    IsAbsent = oSqlDataReader["IsAbsent"].ToBool(),
                                    RollNo = oSqlDataReader["Roll_No"].ToInt(),
                                    UserId = oSqlDataReader["User_Id"].ToInt(),
                                    IssueTiming = oSqlDataReader["IssueTime"].ToDateTime(),
                                    GrNo = oSqlDataReader["Enrolment_Number"].ToString()
                                }
                            );
                    }
                }
                return lstLibraryRecord;
            }
        }

        public void SaveBookDetails(string sStudentBookDetailsXML, DateTime dtBookIssueReturnDate, int iStatus)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BookIssueReturnDate", dtBookIssueReturnDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("StudentBookDetailsXML", sStudentBookDetailsXML, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StatusId", iStatus, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentBookDetails");
            }
        }
    }
}
