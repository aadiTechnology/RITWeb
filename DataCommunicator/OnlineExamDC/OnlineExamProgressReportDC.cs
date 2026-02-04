using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities.OnlineExam;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator.OnlineExamDC
{
    public class OnlineExamProgressReportDC
    {
        int miSchoolId;
        int miAcademicYearId;
        OnlineExamProgressReportDetails moOnlineExamProgressReportDetails;

        public OnlineExamProgressReportDC() { }
        public OnlineExamProgressReportDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }

        public OnlineExamProgressReportDetails GetDetails(int aiStdDivId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);

                moOnlineExamProgressReportDetails = new OnlineExamProgressReportDetails();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetMarksForOnlineProgressReport"))
                {   
                    FillSchoolInfo(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillStudentInfo(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillExams(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillSubjects(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    Fillmarks(oSqlDataReader);
                }

                return moOnlineExamProgressReportDetails;
            }
        }

        private void Fillmarks(SqlDataReader aoSqlDataReader)
        {
            moOnlineExamProgressReportDetails.MarkInformation = new List<MarkInfo>();
            while (aoSqlDataReader.Read())
            {
                moOnlineExamProgressReportDetails.MarkInformation.Add(new MarkInfo
                {
                    Marks = aoSqlDataReader["Marks"].ToInt(),
                    OutOfMarks = aoSqlDataReader["OutOfMarks"].ToInt(),
                    OnlineExamConfigurationId = aoSqlDataReader["OnlineExamConfigurationId"].ToInt(),
                    StudentId = aoSqlDataReader["StudentId"].ToInt(),
                    ExamId = aoSqlDataReader["ExamId"].ToInt(),
                    SubjectId = aoSqlDataReader["SubjectId"].ToInt()
                });
            }
        }

        private void FillSubjects(SqlDataReader aoSqlDataReader)
        {
            moOnlineExamProgressReportDetails.Subjects = new List<SubjectInfo>();
            while (aoSqlDataReader.Read())
            {
                moOnlineExamProgressReportDetails.Subjects.Add(new SubjectInfo
                {
                    Name = aoSqlDataReader["Subject_Name"].ToString(),
                    SubjectId = aoSqlDataReader["Subject_Id"].ToInt(),
                    SortOrder = aoSqlDataReader["Sort_Order"].ToInt()
                });
            }
        }

        private void FillExams(SqlDataReader aoSqlDataReader)
        {
            moOnlineExamProgressReportDetails.OnlineExams = new List<OnlineExam>();
            while (aoSqlDataReader.Read())
            {
                moOnlineExamProgressReportDetails.OnlineExams.Add(new OnlineExam
                {
                    Name = aoSqlDataReader["Name"].ToString(),
                  //  OnlineExamConfigurationId = aoSqlDataReader["OnlineExamConfigurationId"].ToInt(),
                    Id = aoSqlDataReader["Id"].ToInt()
                });
            }
        }

        private void FillStudentInfo(SqlDataReader aoSqlDataReader)
        {
            moOnlineExamProgressReportDetails.Students = new List<StudentInfo>();
            while (aoSqlDataReader.Read())
            {
                moOnlineExamProgressReportDetails.Students.Add(new StudentInfo
                {
                    StudentId = aoSqlDataReader["YearWise_Student_Id"].ToInt(),
                    StudentName = aoSqlDataReader["StudentName"].ToString(),
                    ClassName = aoSqlDataReader["ClassName"].ToString(),
                    AcademicYear = aoSqlDataReader["AcademicYear"].ToString(),
                    RollNo = aoSqlDataReader["Roll_No"].ToInt()
                });
            }
        }

        private void FillSchoolInfo(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                moOnlineExamProgressReportDetails.SchoolInformation = new SchoolInfo
                {
                    SchoolName = aoSqlDataReader["School_Name"].ToString(),
                    OrgName = aoSqlDataReader["School_Orgn_Name"].ToString()
                };
            }
        }
    }
}
