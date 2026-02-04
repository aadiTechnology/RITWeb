using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities.ProgressReport;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class ExamReporstDC
    {
        public ExamReporstDC()
        {
        }

        public TestwiseMark GetMarkDetailsForTestwiseReport(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStdDivId, int aiTestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetMarksForTestwiseConsolidatedReport"))
                {
                    TestwiseMark oTestwiseMark = new TestwiseMark();
                    oTestwiseMark.StudentDetails = new List<StudentDetailsForTestReport>();
                    while (oSqlDataReader.Read())
                    {
                        oTestwiseMark.StudentDetails.Add(new StudentDetailsForTestReport {
                            RollNo = oSqlDataReader["Roll_No"].ToInt(),
                            StudentName = oSqlDataReader["StudentName"].ToString(),
                            YearWiseStudentId = oSqlDataReader["YearWise_Student_Id"].ToInt()
                        });
                    }

                    oSqlDataReader.NextResult();

                    oTestwiseMark.Marks = new List<MarkDetails>();
                    while (oSqlDataReader.Read())
                    {
                        oTestwiseMark.Marks.Add(new MarkDetails
                        {
                            Percentage = oSqlDataReader["Percentage"].ToDecimal(),
                            //OutOfMarks = oSqlDataReader["OutOfMarks"].ToInt(),
                            SchoolWiseTestId = oSqlDataReader["SchoolWise_Test_Id"].ToInt(),
                            StudentId = oSqlDataReader["Student_Id"].ToInt(),
                            TotalMarksScored = oSqlDataReader["Total_Marks_Scored"].ToDecimal(),
                            SubjectId = oSqlDataReader["Subject_Id"].ToInt(),
                            IsAbsent = oSqlDataReader["IsAbsent"].ToString(),
                            SchoolWiseStudentTestMarksId = oSqlDataReader["SchoolWiseStudentTestMarksId"].ToInt()
                        });
                    }

                    oSqlDataReader.NextResult();
                    oTestwiseMark.Subjects = new List<SubjectForTestReport>();
                    while (oSqlDataReader.Read())
                    {
                        oTestwiseMark.Subjects.Add(new SubjectForTestReport {
                            IsGradeApplicable = oSqlDataReader["IsGradeApplicable"].ToBool(),
                            SortOrder = oSqlDataReader["Sort_Order"].ToInt(),
                            SubjectName = oSqlDataReader["Subject_Name"].ToString(),
                            SubjectId = oSqlDataReader["Subject_Id"].ToInt(),
                            ParentSubjectId = oSqlDataReader["ParentSubjectId"].ToInt(),
                            ParentSubjectName = oSqlDataReader["ParentSubjectName"].ToString(),
                            SubjectTotalMarks = oSqlDataReader["SubjectTotalMarks"].ToInt()
                        });
                    }
                    
                    oSqlDataReader.NextResult();
                    oTestwiseMark.Exams = new List<TestForTestReport>();
                    while (oSqlDataReader.Read())
                    {
                        oTestwiseMark.Exams.Add(new TestForTestReport
                        {
                            SortOrder = oSqlDataReader["Sort_Order"].ToInt(),
                            SchoolWiseTestName = oSqlDataReader["SchoolWise_Test_Name"].ToString(),
                            SchoolWiseTestId = oSqlDataReader["SchoolWise_Test_Id"].ToInt()
                        });
                    }

                    oSqlDataReader.NextResult();
                    oTestwiseMark.Grades = new List<GradeDetailsForTestReport>();
                    while (oSqlDataReader.Read())
                    {
                        oTestwiseMark.Grades.Add(new GradeDetailsForTestReport
                        {
                            StartingMarkRange = oSqlDataReader["Starting_Marks_Range"].ToInt(),
                            EndingMarkRange = oSqlDataReader["Actual_Ending_Marks_Range"].ToDecimal(),
                            GradeName = oSqlDataReader["Grade_Name"].ToString()
                        });
                    }

                    oSqlDataReader.NextResult();
                    oTestwiseMark.MarkSummary = new List<TestSummaryDetails>();
                    while (oSqlDataReader.Read())
                    {
                        oTestwiseMark.MarkSummary.Add(new TestSummaryDetails
                        {
                            StudentId = oSqlDataReader["Student_Id"].ToInt(),
                            Percentage = oSqlDataReader["Percentage"].ToDecimal(),
                            TotalMarks = oSqlDataReader["TotalMarks"].ToInt(),
                            Rank = oSqlDataReader["Rank"].ToInt()
                        });
                    }

                    oSqlDataReader.NextResult();
                    oTestwiseMark.OtherDetails = new ClassDetails();
                    if (oSqlDataReader.Read())
                    {
                        oTestwiseMark.OtherDetails.ClassName = oSqlDataReader["ClassName"].ToString();
                        oTestwiseMark.OtherDetails.TeacherName = oSqlDataReader["TeacherName"].ToString();
                        oTestwiseMark.OtherDetails.SchoolName = oSqlDataReader["School_Name"].ToString();
                        oTestwiseMark.OtherDetails.TestName = oSqlDataReader["TestName"].ToString();
                        oTestwiseMark.OtherDetails.DisplaySubTypes = oSqlDataReader["DisplaySubTypes"].ToBool();
                    }

                    oSqlDataReader.NextResult();
                    oTestwiseMark.TestTypeMarkDetails = new List<TestTypeMarks>();
                    while (oSqlDataReader.Read())
                    {
                        oTestwiseMark.TestTypeMarkDetails.Add(new TestTypeMarks
                        {
                            SchoolWiseStudentTestMarksId = oSqlDataReader["SchoolWiseStudentTestMarksId"].ToInt(),
                            IsAbsent = oSqlDataReader["IsAbsent"].ToString(),
                            Marks_Scored = oSqlDataReader["Marks_Scored"].ToDecimal(),
                            SortOrder = oSqlDataReader["SortOrder"].ToInt(),
                            TestTypeId = oSqlDataReader["TestTypeId"].ToInt(),
                            TestTypeName = oSqlDataReader["TestTypeName"].ToString(),
                            OutOfMarks = oSqlDataReader["OutOfMarks"].ToInt()                            
                        });
                    }

                    return oTestwiseMark;
                }                
            }
        }
    }
}
