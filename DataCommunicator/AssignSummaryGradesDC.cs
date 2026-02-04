using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.Teacher;

namespace DataCommunicator
{
    public class AssignSummaryGradesDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;
        private string msTestName;
        private string msSubjectName;
        private ButtonStatesforAssignSummaryGrades moButtonStatesforAssignSummaryGrades;

        #endregion

        #region Constructor(s)

        public AssignSummaryGradesDC(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
            this.miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region Property(s)

        public ButtonStatesforAssignSummaryGrades ButtonStates
        {
            get { return moButtonStatesforAssignSummaryGrades; }
        }

        public string TestName
        {
            get { return msTestName; }
        }

        public string SubjectName
        {
            get { return msSubjectName; }
        }

        #endregion

        #region Public Method(s)
        /// <summary>
        /// these method is used to  get students to fill listview. 
        /// </summary>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiTestId"></param>
        /// <returns></returns>
        public List<AssignSummaryGradesDetails> GetAll(int aiStandardDivId, int aiSubjectId, int aiTestId)
        {
            List<AssignSummaryGradesDetails> lstAssignSummaryGradesDetails = new List<AssignSummaryGradesDetails>();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivId", aiStandardDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllClasswiseStudentsForSummaryGrades"))
                {
                    moButtonStatesforAssignSummaryGrades = new ButtonStatesforAssignSummaryGrades();

                    while (oSqlDataReader.Read())
                    {
                        lstAssignSummaryGradesDetails.Add(new AssignSummaryGradesDetails
                        {
                            RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                            StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                            YearwiseStudentId = Convert.ToInt32(oSqlDataReader["YearWise_Student_Id"]),
                            GradeId = Convert.ToInt32(oSqlDataReader["GradeId"]),
                        });
                    }

                    if (oSqlDataReader.NextResult() && oSqlDataReader.Read())
                    {
                        moButtonStatesforAssignSummaryGrades.IsSaved = Convert.ToBoolean(oSqlDataReader["IsSaved"]);
                        moButtonStatesforAssignSummaryGrades.IsSubmitted = Convert.ToBoolean(oSqlDataReader["IsSubmitted"]);
                        moButtonStatesforAssignSummaryGrades.IsPublished = Convert.ToBoolean(oSqlDataReader["IsPublished"]);

                        msSubjectName = Convert.ToString(oSqlDataReader["ExamName"]);
                        msTestName = Convert.ToString(oSqlDataReader["TestName"]);
                    }
                }

                return lstAssignSummaryGradesDetails;
            }
        }

        /// <summary>
        /// this method is used to save students grade details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiTestId"></param>
        public void Save(string asXml, int aiSubjectId, int aiTestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentGradeDetailxml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentsObservationSummaryGradeDetails");
            }
        }

        /// <summary>
        /// this method is used to  submit and unsubmit grade details
        /// </summary>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiTestId"></param>
        /// <param name="abIsSubmitted"></param>
        public void Submit(int aiStandardDivId, int aiSubjectId, int aiTestId, bool abIsSubmitted)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivId", aiStandardDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmitted", abIsSubmitted, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitStudentSummaryGradeDetails");
            }
        }

        #endregion
    }
}
