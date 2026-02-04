using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;

namespace DataCommunicator
{
    public class StudentExamWiseSubjectMarksDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;

        #endregion

        #region Constructor(s)

        public StudentExamWiseSubjectMarksDetailsDC(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
            this.miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// These method returns exams
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public List<StudentExamWiseSubjectMarksDetails> GetExams(int aiStudentId)
        {
            List<StudentExamWiseSubjectMarksDetails> lstStudentExamWiseSubjectMarksDetails = new List<StudentExamWiseSubjectMarksDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_GetAllExamsOfStudent]"))
                {
                    while (oSqlDataReader.Read())
                    {
                        StudentExamWiseSubjectMarksDetails oObservationParameters = new StudentExamWiseSubjectMarksDetails
                        {
                            TestId = Convert.ToInt32(oSqlDataReader["SchoolWise_Test_Id"]),
                            TestName = oSqlDataReader["SchoolWise_Test_Name"].ToString()
                        };
                        lstStudentExamWiseSubjectMarksDetails.Add(oObservationParameters);
                    }
                }
            }
            return lstStudentExamWiseSubjectMarksDetails;
        }

        /// <summary>
        /// these method return subjects and marks according to selected exam
        /// </summary>
        /// <param name="aiTestId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public List<StudentExamWiseSubjectMarksDetails> GetAllSubjects(int aiTestId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllStudentExamwiseSubjectMarksDetails"))
                    return this.FillStudentExamWiseSubjectMarksDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to fill Student Subject Marks Details entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<StudentExamWiseSubjectMarksDetails> FillStudentExamWiseSubjectMarksDetails(SqlDataReader aoSqlDataReader)
        {
            List<StudentExamWiseSubjectMarksDetails> lstStudentExamWiseSubjectMarksDetails = new List<StudentExamWiseSubjectMarksDetails>();
            while (aoSqlDataReader.Read())
            {
                lstStudentExamWiseSubjectMarksDetails.Add(new StudentExamWiseSubjectMarksDetails
                {

                    SubjectName = Convert.ToString(aoSqlDataReader["Subject_Name"]),
                    Marks = Convert.ToDecimal(aoSqlDataReader["Total_Marks_Scored"]),
                    OutOfMarks = Convert.ToDecimal(aoSqlDataReader["OutOfMarks"]),
                    Grade = Convert.ToString(aoSqlDataReader["Grade_Name"]),
                    IsAbsentGrade = Convert.ToString(aoSqlDataReader["Is_Absent"]),
                    IsGradingSubject = Convert.ToBoolean(aoSqlDataReader["IsGradingSubject"])
                });
            }

            return lstStudentExamWiseSubjectMarksDetails;
        }

        #endregion
    }
}

