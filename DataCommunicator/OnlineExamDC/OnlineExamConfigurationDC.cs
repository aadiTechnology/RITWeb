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
namespace DataCommunicator
{
  public  class OnlineExamConfigurationDC
    {
        #region Data Member(s)

        public int miSchoolId;
        public int miAcademicYearId;
        private int miUpdatedById; 

        #endregion

        #region Constructor(s)

        public OnlineExamConfigurationDC() { }

        public OnlineExamConfigurationDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUpdatedById = aiUpdatedById;
        } 

        #endregion

        public DataTable GetAllQuestions(int aiStandardId, int aiStandardDivisionId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllExamQuestions");
            }
        }
        public DataSet GetDetailsForUpdateQuestions(int aiVehicleId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetDetailsForEditQuestions");

            }
        }

        public DataTable GetAllTestsForClass()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllTestsForOnlineExam");
            }
        }

        public DataTable GetAllTestsForStudent(int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllTestsForOnlineExam");
            }
        }

        public DataTable GetAssociatedStandards(int aiStandardId)
        {
            string sSelectStatement = " SELECT " +
                                    "School_Id" +
                                    ",Standard_Id " +
                                    ",className as class_name " +
                                    ",SchoolWise_Standard_Division_Id" +
                                     ",Division_Name" +
                                    " FROM " +
                                    "vw_standard_division " +
                                    " WHERE " +
                                    "School_Id =" + this.miSchoolId +
                                    "AND academic_year_id =" + this.miAcademicYearId +
                                     "AND Standard_Id =" + aiStandardId +
                                    "ORDER BY " +
                                    "Original_Division_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
        public DataTable GetAllStandards()
        {
            string sSelectStatement = " SELECT " +
                                    "School_Id" +
                                    ",Standard_Id " +
                                    ",Standard_Name " +
                                    
                                    " FROM " +
                                    "Standard_Master " +
                                    " WHERE " +
                                    "School_Id =" + this.miSchoolId +
                                    "AND academic_year_id =" + this.miAcademicYearId +
                                    "ORDER BY " +
                                    "Original_Standard_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
        public List<YearWiseSubjectsDetails> GetAllYearwiseSubjects(int aiStdId, int aiStdDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdId", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetYearwiseSubjectList"))
                    return this.ReadAllSubjects(oSqlDataReader);
            }
        }
        public List<YearWiseSubjectsDetails> ReadAllSubjects(SqlDataReader aoSqlDataReader)
        {
            List<YearWiseSubjectsDetails> lstSubjectsDetails = new List<YearWiseSubjectsDetails>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    YearWiseSubjectsDetails oYearWiseSubjectsDetails = new YearWiseSubjectsDetails();
                    if (aoSqlDataReader["Subject_Id"] != DBNull.Value)
                        oYearWiseSubjectsDetails.SubjectId = Convert.ToInt32(aoSqlDataReader["Subject_Id"]);
                    if (aoSqlDataReader["Subject_Name"] != DBNull.Value)
                        oYearWiseSubjectsDetails.SubjectName = aoSqlDataReader["Subject_Name"].ToString();

                    lstSubjectsDetails.Add(oYearWiseSubjectsDetails);
                }
                aoSqlDataReader.Close();
            }
            return lstSubjectsDetails;
        }
        public void Save(string asStaffXML, OnlineExamConfiguration oExamConfig, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", oExamConfig.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", oExamConfig.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamId", oExamConfig.ExamId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", oExamConfig.SubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", oExamConfig.StandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamXML", asStaffXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("InsertedById", oExamConfig.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDateAndTime", oExamConfig.StartDateAndTime, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EndDateAndTime", oExamConfig.EndDateAndTime, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("NoOfQuestions", oExamConfig.NoOfQuestions, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShuffleForCount", oExamConfig.ShuffleForCount, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("ShuffleForSequence", oExamConfig.ShuffleForSequence, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StartTime", oExamConfig.StartTime, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EndTime", oExamConfig.EndTime, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Id", oExamConfig.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveOnlineExamWiseQueConfig");

            }
        }
        public void Delete(int aiConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteOnlineQuestionDetails");                
            }
        }
        public List<OnlineExamConfiguration> GetAllExamQuestionConfiguration(int aiSchoolId, int aiAcademicYearId, String sSortExpression,string sortDirection, int iEndIndex, int startRowIndex,int aiStandardId, int aiStandardDivisionId, int aiSubjectId)
        {   
            if (sSortExpression == "")
                sSortExpression = " StartDateAndTime";

            sSortExpression = sSortExpression.ToLower().Replace(Constants.S_ASCENDING, string.Empty).Replace(Constants.S_DESCENDING, string.Empty);

            sSortExpression = " Order By " + sSortExpression + " " + sortDirection;

            List<OnlineExamConfiguration> lst = new List<OnlineExamConfiguration>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                //oSQLServerDbUtility.AddParameter("SortExp", sSortExpression == "" ? "" : " ORDER BY " + sSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExp", sSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllExamQuestionDetails"))
                {
                    if (oReader.HasRows)
                    {
                        while (oReader.Read())
                            lst.Add(ReadObjectFromReader(oReader));
                    }
                }

            }
            return lst;

        }
        //public static int CountTotalExamQuestionConfiguration(Int32 aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex, int aiStandardDivisionId, int aiSubjectId)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
        //        SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
        //        oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
        //        oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCountAllExamQuestionDetails");
        //        return Convert.ToInt32(oSqlParameter.Value);
        //    }
        //}
        private OnlineExamConfiguration ReadObjectFromReader(SqlDataReader aoReader)
        {
            return new OnlineExamConfiguration()
            {
                Class = aoReader["ClassName"].ToString(),
                Exam = aoReader["TestName"].ToString(),
                Subject = aoReader["Subject_Name"].ToString(),                
                StartDateAndTime = aoReader["StartDateAndTime"].ToDateTime(),
                EndDateAndTime = aoReader["EndDateAndTime"].ToDateTime(),
                NoOfQuestions = aoReader["NoOfQuestions"].ToInt(),
                Id = aoReader["Id"].ToInt(),
                TotalRows = aoReader["TotalRows"].ToInt(),
                IsSubmitted = aoReader["IsSubmitted"].ToBool()
            };
        }

        //public DataTable CopyExamConfigurationForClasses(int aitargetstddivid, int aisubject, string sids, string ExamXML)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("SourceStandardDivisionId", aitargetstddivid, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("SubjectId", aisubject, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("TargetStandardDivisionIds", sids, SqlDbType.NVarChar);
        //        oSQLServerDbUtility.AddParameter("ExamXML", ExamXML, SqlDbType.Xml);
        //        return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CopyExamConfigurationForClasses");
        //    };
        //}


        public ButtonStateDetails GetButtonState(int aiStdId, int aiStdDivId, int aiSubjectId)
        {
            ButtonStateDetails oButtonStateDetails = new ButtonStateDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdId", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOnlineExamSubmitStatus"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oButtonStateDetails.EnableSubmitButtton = oSqlDataReader["EnableSubmitButtton"].ToBool();
                        oButtonStateDetails.EnableUnSubmitButtton = oSqlDataReader["EnableUnSubmitButtton"].ToBool();
                    }
                }
            }
            return oButtonStateDetails;
        }

        public void Submit(int aiStdId, int aiStdDivId, int aiSubjectId, bool abIsSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdId", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", abIsSubmit, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitOnlineExam");
            }
        }
    }
}
