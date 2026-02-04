using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.ProgressReport;

namespace DataCommunicator
{
    public class StudentMarksDC
    {
        #region Member(s)
        
        private int miSchoolId, miAcademicYearId, miUpdatedBYId; 

        #endregion

        #region Constructor(s)

        public StudentMarksDC()
        {
        }

        public StudentMarksDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedBYId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedBYId = aiUpdatedBYId;
        } 

        #endregion

        #region Public MEthod(s)

        /// <summary>
        /// This method is used to return mark details.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiTestId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public StudentConsolidatedMarkDetails GetAllDetails(int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiTestId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                StudentConsolidatedMarkDetails oStudentConsolidatedMarkDetails = new StudentConsolidatedMarkDetails();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetConsolidatedMarkDetails"))
                {
                    oStudentConsolidatedMarkDetails.Marks = GetMarks(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    oStudentConsolidatedMarkDetails.ExamConfigs = GetExamConfig(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    oStudentConsolidatedMarkDetails.Students = GetStudents(oSqlDataReader);


                    oSqlDataReader.NextResult();
                    oStudentConsolidatedMarkDetails.ExamStatusConfigs = GetExamConfigStatus(oSqlDataReader);
                }

                return oStudentConsolidatedMarkDetails;
            }
        }

        /// <summary>
        /// This method is used to return test list.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public List<Test> GetAllTestsForClassSUbject(int aiAcademicYearId, int aiStdDivId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllTestsForClassSUbject"))
                {
                    List<Test> lstTest = new List<Test>();
                    while (oSqlDataReader.Read())
                    {
                        lstTest.Add(new Test { Name = oSqlDataReader["SchoolWise_Test_Name"].ToString(), TestId = Convert.ToInt32(oSqlDataReader["SchoolWise_Test_Id"]) });
                    }
                    return lstTest;
                }
            }
        } 

        #endregion

        #region Private Methods

        private List<ExamStatusConfig> GetExamConfigStatus(SqlDataReader aoSqlDataReader)
        {
            List<ExamStatusConfig> lstExamStatusConfig = new List<ExamStatusConfig>();
            while (aoSqlDataReader.Read())
            {
                lstExamStatusConfig.Add(new ExamStatusConfig
                {
                    ConsiderInTotal = Convert.ToString(aoSqlDataReader["ConsiderInTotal"]),
                    DisplayTotal = Convert.ToString(aoSqlDataReader["DisplayTotal"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                    DisplayValue = Convert.ToString(aoSqlDataReader["DisplayValue"])
                });
            }
            return lstExamStatusConfig;
        }

        private List<StudentInfo> GetStudents(SqlDataReader aoSqlDataReader)
        {
            List<StudentInfo> lstStudentInfo = new List<StudentInfo>();
            while (aoSqlDataReader.Read())
            {
                lstStudentInfo.Add(new StudentInfo
                {
                    StudentId = Convert.ToInt32(aoSqlDataReader["YearWise_Student_Id"]),
                    RollNo = Convert.ToInt32(aoSqlDataReader["Roll_No"]),
                    Name = Convert.ToString(aoSqlDataReader["StudentName"]),
                });
            }
            return lstStudentInfo;
        }

        private List<ExamConfig> GetExamConfig(SqlDataReader aoSqlDataReader)
        {
            List<ExamConfig> lstExamConfig = new List<ExamConfig>();
            while (aoSqlDataReader.Read())
            {
                lstExamConfig.Add(new ExamConfig
                {
                    SchoolWiseTestId = Convert.ToInt32(aoSqlDataReader["SchoolWise_Test_Id"]),
                    SchoolWiseTestName = Convert.ToString(aoSqlDataReader["SchoolWise_Test_Name"]),
                    SubjectId = Convert.ToInt32(aoSqlDataReader["Subject_Id"]),
                    SubjectName = Convert.ToString(aoSqlDataReader["Subject_Name"]),
                    SubjectSortOrder = Convert.ToInt32(aoSqlDataReader["SubjectSortOrder"]),
                    SubjectTotalMarks = Convert.ToInt32(aoSqlDataReader["Subject_Total_Marks"]),
                    TestSortOrder = Convert.ToInt32(aoSqlDataReader["TestSortOrder"]),
                    TestWiseSubjectMarksId = Convert.ToInt32(aoSqlDataReader["TestWise_Subject_Marks_Id"]),
                });
            }

            return lstExamConfig;
        }

        private List<Mark> GetMarks(SqlDataReader aoSqlDataReader)
        {
            List<Mark> mlstMarks = new List<Mark>();
            while (aoSqlDataReader.Read())
            {
                mlstMarks.Add(new Mark
                    {
                        SchoolWiseTestId = Convert.ToInt32(aoSqlDataReader["SchoolWise_Test_Id"]),
                        SubjectId = Convert.ToInt32(aoSqlDataReader["Subject_Id"]),
                        StudentId = Convert.ToInt32(aoSqlDataReader["Student_Id"]),
                        SubjectTotalMarks = Convert.ToInt32(aoSqlDataReader["Subject_Total_Marks"]),
                        TestWiseSubjectMarksId = Convert.ToInt32(aoSqlDataReader["TestWise_Subject_Marks_Id"]),
                        TotalMarksScored = Convert.ToInt32(aoSqlDataReader["Total_Marks_Scored"]),
                        IsAbsent = Convert.ToChar(aoSqlDataReader["Is_Absent"])
                    });
            }

            return mlstMarks;
        }

        

        #endregion
    }
}
