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
    public class ExportReportDC
    {
        #region Data Member(s)

        private List<SubjectInfo> mlstSubjects;
        private List<TestDetails> mlstTestDetails;
        private List<StudentInfoForExam> mlstStudentInfo;
        private List<StudentMarkSummary> mlstStudentMarkSummary;
        private BasicInfo moBasicInfo;
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById; 

        #endregion

        #region Constructure(s)

        public ExportReportDC()
        {
        }

        public ExportReportDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Property(s)

        public List<SubjectInfo> Subjects
        {
            get
            {
                return this.mlstSubjects;
            }
        }

        public List<TestDetails> TestDetails
        {
            get
            {
                return this.mlstTestDetails;
            }
        }

        public List<StudentInfoForExam> StudentInfos
        {
            get
            {
                return this.mlstStudentInfo;
            }
        }
        public BasicInfo BasicInfo
        {
            get
            {
                return this.moBasicInfo;
            }
        }

        public List<StudentMarkSummary> StudentMarkSummary
        {
            get
            {
                return this.mlstStudentMarkSummary;
            }
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return result sheet details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiTestId"></param>
        /// <returns></returns>
        public List<StudentMarkDetails> GetResultSheetDetails(int aiStandardId, int aiDivisionId, int aiTestId, bool abIsPrelimReport, int aiTermId)
        {
            List<StudentMarkDetails> lstStudentMarkDetails = new List<StudentMarkDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Test_Id", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPrelimReport", abIsPrelimReport, SqlDbType.Bit);                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetResultSheetDetails"))
                {
                    LoadBasicDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadStudentDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadSubjectDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillMarkSummeryDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    return LoadMarkDetails(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to return result sheet details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiTestId"></param>
        /// <returns></returns>
        public List<StudentMarkDetails> GetAnnualConsolDetailsForHSP(int aiStandardId, int aiDivisionId)
        {
            List<StudentMarkDetails> lstStudentMarkDetails = new List<StudentMarkDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAnnualConsolDetailsForHSP"))
                {
                    LoadBasicDetailsForHSP(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadStudentDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadSubjectDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadTestDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillMarkSummeryDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    return LoadMarkDetailsForHSPReport(oSqlDataReader);
                }
            }
        }

        private void LoadTestDetails(SqlDataReader aoSqlDataReader)
        {
            this.mlstTestDetails = new List<TestDetails>();
            while (aoSqlDataReader.Read())
            {
                this.mlstTestDetails.Add(new TestDetails
                {
                    SchoolwiseTestId = aoSqlDataReader["Schoolwise_Test_Id"].ToInt(),
                    TestName = aoSqlDataReader["TestName"].ToString(),
                    TestSortOrder = aoSqlDataReader["TestSortOrder"].ToInt(),
                    OutOfMarks = aoSqlDataReader["OutOfMarks"].ToInt(),
                    GroupSortOrder = aoSqlDataReader["GroupSortOrder"].ToInt(),
                    TermId = aoSqlDataReader["TermId"].ToInt()
                });
            }
        } 

        #endregion

        #region Private method(s)

        /// <summary>
        /// This method is used to load mark summary details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillMarkSummeryDetails(SqlDataReader aoSqlDataReader)
        {
            this.mlstStudentMarkSummary = new List<StudentMarkSummary>();
            while (aoSqlDataReader.Read())
            {
                this.mlstStudentMarkSummary.Add(new StudentMarkSummary
                {
                    OutOfMarks = aoSqlDataReader["OutOfMarks"].ToInt(),
                    StudentId = aoSqlDataReader["Student_Id"].ToInt(),
                    TotalScoredMarks = aoSqlDataReader["TotalScoredMarks"].ToDecimal(),
                    Percentage = aoSqlDataReader["Percentage"].ToDecimal(),
                    Rank = aoSqlDataReader["Rank"].ToInt(),
                    Grade = aoSqlDataReader["Grade"].ToString()
                });
            }
        }

        /// <summary>
        /// This method is used to load mark details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StudentMarkDetails> LoadMarkDetails(SqlDataReader aoSqlDataReader)
        {
            List<StudentMarkDetails> lstStudentMarkDetails = new List<StudentMarkDetails>();
            while (aoSqlDataReader.Read())
            {
                lstStudentMarkDetails.Add(new StudentMarkDetails
                {
                    SubjectId = aoSqlDataReader["Subject_Id"].ToInt(),
                    StudentId = aoSqlDataReader["Student_Id"].ToInt(),
                    OutOfMarks = aoSqlDataReader["OutOfMarks"].ToInt(),
                    ScoredMarks = aoSqlDataReader["Total_Marks_Scored"].ToDecimal(),
                    Grade = aoSqlDataReader["Grade"].ToString(),
                    ExamStatus = aoSqlDataReader["ExamStatus"].ToString()
                });
            }
            return lstStudentMarkDetails;
        }

        private List<StudentMarkDetails> LoadMarkDetailsForHSPReport(SqlDataReader aoSqlDataReader)
        {
            List<StudentMarkDetails> lstStudentMarkDetails = new List<StudentMarkDetails>();
            while (aoSqlDataReader.Read())
            {
                lstStudentMarkDetails.Add(new StudentMarkDetails
                {
                    SubjectId = aoSqlDataReader["Subject_Id"].ToInt(),
                    StudentId = aoSqlDataReader["Student_Id"].ToInt(),
                    OutOfMarks = aoSqlDataReader["OutOfMarks"].ToInt(),
                    ScoredMarks = aoSqlDataReader["Total_Marks_Scored"].ToDecimal(),
                    Grade = aoSqlDataReader["Grade"].ToString(),
                    ExamStatus = aoSqlDataReader["ExamStatus"].ToString(),
                    SchoolwiseTestId = aoSqlDataReader["SchoolwiseTestId"].ToInt()
                    
                });
            }
            return lstStudentMarkDetails;
        }

        /// <summary>
        /// This method is used to load subject details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadSubjectDetails(SqlDataReader aoSqlDataReader)
        {
            this.mlstSubjects = new List<SubjectInfo>();
            while (aoSqlDataReader.Read())
            {
                this.mlstSubjects.Add(new SubjectInfo
                {
                    SubjectId = aoSqlDataReader["Subject_Id"].ToInt(),
                    SubjectName = aoSqlDataReader["Subject_Name"].ToString(),
                    SortOrder = aoSqlDataReader["Sort_Order"].ToInt(),
                    ParentSubject = aoSqlDataReader["ParentSubject"].ToString(),
                    IsCoCurricularSubject = aoSqlDataReader["IsCoCurricularSubject"].ToBool()
                });
            }
        }

        /// <summary>
        /// This method is used to load student details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadStudentDetails(SqlDataReader aoSqlDataReader)
        {
            this.mlstStudentInfo = new List<StudentInfoForExam>();
            while (aoSqlDataReader.Read())
            {
                this.mlstStudentInfo.Add(new StudentInfoForExam
                {
                    StudentId = aoSqlDataReader["YearWise_Student_Id"].ToInt(),
                    StudentName = aoSqlDataReader["StudentName"].ToString(),
                    HouseName = aoSqlDataReader["HouseName"].ToString(),
                    RollNo = aoSqlDataReader["Roll_No"].ToInt(),
                    OriginalDivisionId = aoSqlDataReader["Original_Division_Id"].ToInt()
                });
            }
        }

        /// <summary>
        /// This method is used to load basic details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadBasicDetailsForHSP(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                this.moBasicInfo = new BasicInfo
                {
                    AcademicYear = aoSqlDataReader["AcademicYear"].ToString(),
                    ClassName = aoSqlDataReader["ClassName"].ToString(),
                    Location = aoSqlDataReader["Location"].ToString(),
                    SchoolName = aoSqlDataReader["SchoolName"].ToString(),
                    TestName = aoSqlDataReader["TestName"].ToString(),
                    ShowGrades = aoSqlDataReader["ShowGrades"].ToBool(),
                    OrgName = aoSqlDataReader["OrgName"].ToString(),
                    PrincipalName = aoSqlDataReader["PrincipalName"].ToString(),
                    ClassTeacherName = aoSqlDataReader["ClassTeacherName"].ToString()
                };
            }
        } 

        /// <summary>
        /// This method is used to load basic details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadBasicDetails(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                this.moBasicInfo = new BasicInfo
                {
                    AcademicYear = aoSqlDataReader["AcademicYear"].ToString(),
                    ClassName = aoSqlDataReader["ClassName"].ToString(),
                    Location = aoSqlDataReader["Location"].ToString(),
                    SchoolName = aoSqlDataReader["SchoolName"].ToString(),
                    TestName = aoSqlDataReader["TestName"].ToString(),
                    ShowGrades = aoSqlDataReader["ShowGrades"].ToBool()
                };
            }
        } 

        #endregion
    }
}
