using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.Teacher;

namespace DataCommunicator
{
   public class StudentListForNoteDetailsDC
    {
       #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;

        #endregion

        #region Constructor(s)

        public StudentListForNoteDetailsDC(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
            this.miAcademicYearId = aiAcademicYearId;
        }

        #endregion Constructor(s)

        public List<StudentListForNoteDetails> GetAllStudentList( int aiStandardId, int aiDivisionId)
        {
            List<StudentListForNoteDetails> lstStudentListForNoteDetails = new List<StudentListForNoteDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllStudentsList"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstStudentListForNoteDetails.Add(new StudentListForNoteDetails
                        {
                            SchoolWiseStudentId = Convert.ToInt32(oSqlDataReader["SchoolWise_Student_Id"]),
                            GRNumber = oSqlDataReader["GRNumber"].ToString(),
                            RollNumber = oSqlDataReader["Roll_No"].ToString(),
                            studentName = oSqlDataReader["StudentName"].ToString(),
                         });
                    }
                }
            }
            return lstStudentListForNoteDetails;
        }
    }
}
