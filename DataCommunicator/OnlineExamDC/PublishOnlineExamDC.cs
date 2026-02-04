using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;
using DataCommunicator;
using Utility;
using SchoolEntities.Admin;
using SchoolEntities.OnlineExam;

namespace DataCommunicator
{
   public class PublishOnlineExamDC
    {
        #region " Data Members "

        public int miSchoolId;
        public int miAcademicYearId;
        private int miUpdatedById;
        List<OnlineExamResult> mlstExamResult;
        bool mbIsPublished;
        bool mbAllowPublish;
        #endregion
        public PublishOnlineExamDC() { }

        public PublishOnlineExamDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUpdatedById = aiUpdatedById;
        }

        public List<OnlineExamResult> ExamResults
        {
            get { return mlstExamResult; }
        }

        public bool IsPublished
        {
            get { return mbIsPublished; }
        }

        public bool AllowPublish
        {
            get { return mbAllowPublish; }
        }

        public DataTable GetAssociatedStandards(int aiSchoolId, int aiAcademicYearId)
        {

            
            string sSelectStatement = " SELECT " +
                                     "School_Id" +
                                     ",Standard_Id " +
                                     ",className as class_name " +
                                     ",SchoolWise_Standard_Division_Id" +
                                     " FROM " +
                                     "vw_standard_division " +
                                     " WHERE " +
                                     "School_Id =" + miSchoolId +
                                     "AND academic_year_id =" + miAcademicYearId +
                                     "ORDER BY " +
                                     "Original_Standard_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);

        }
        public List<StudentInfo> GetAllStudentsForClass(int aiStdDivId, int aiExamId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamId", aiExamId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
               
                mlstExamResult = new List<OnlineExamResult>();

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllStudentsForClass"))
                {
                    while (oSqlDataReader.Read())
                    {
                        mlstExamResult.Add(new OnlineExamResult
                        { 
                            Answer = oSqlDataReader["Answer"].ToString(),
                            Question = oSqlDataReader["Question"].ToString(),
                            //SortOrder = oSqlDataReader["SortOrder"].ToInt(),
                            StudentId = oSqlDataReader["StudentId"].ToInt(),
                            IsCorrectAnswer = oSqlDataReader["IsCorrectAnswer"].ToBool(),
                            AnswerTypeId = oSqlDataReader["AnswerTypeId"].ToInt(),
                            QuestionAttachmentPath = oSqlDataReader["QuestionAttachmentPath"].ToString(),
                            AnswernAttachmentPath = oSqlDataReader["AnswernAttachmentPath"].ToString(),
                        });
                    }

                    List<StudentInfo> lstStudents = new List<StudentInfo>();
                    oSqlDataReader.NextResult();
                    while (oSqlDataReader.Read())
                    {
                        lstStudents.Add(new StudentInfo
                        {
                            StudentName = oSqlDataReader["StudentName"].ToString(),                            
                            RollNo = oSqlDataReader["Roll_No"].ToInt(),
                            StudentId = oSqlDataReader["YearWise_Student_Id"].ToInt()
                        });
                    }

                    return lstStudents;
                }
            }
        }

        public List<OnlineExamStatus> GetExamResult(int aiStdDivId, int aiExamId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamId", aiExamId, SqlDbType.Int);

                List<OnlineExamStatus> lstOnlineExamStatus = new List<OnlineExamStatus>();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOnlineExamStatus"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstOnlineExamStatus.Add(new OnlineExamStatus { 
                            Absent =oSqlDataReader["Absent"].ToInt(),
                            Present = oSqlDataReader["Present"].ToInt(),
                            Subject = oSqlDataReader["Subject_Name"].ToString(),
                            SubjectId = oSqlDataReader["Subject_Id"].ToInt(),
                            IsPublished = oSqlDataReader["IsPublished"].ToBool(),
                            AnswerTypeId = oSqlDataReader["AnswerTypeId"].ToInt()
                        });
                    }

                    oSqlDataReader.NextResult();
                    if (oSqlDataReader.Read())
                    {
                        mbIsPublished = oSqlDataReader["IsPublished"].ToBool();
                        mbAllowPublish = oSqlDataReader["AllowPublish"].ToBool();
                    }
                }

                return lstOnlineExamStatus;
            }
        }

        public void Publish(int aiStdDivId, int aiExamId, bool abIsPublish)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamId", aiExamId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPublish", abIsPublish, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PublishOnlineExam");
            }
        }
    }
}
